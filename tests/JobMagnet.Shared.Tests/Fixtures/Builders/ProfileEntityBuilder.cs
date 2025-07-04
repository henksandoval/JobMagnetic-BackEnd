using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Fixtures.Customizations;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private readonly IClock _clock;
    private readonly IFixture _fixture;
    private readonly IGuidGenerator _guidGenerator;
    private CareerHistory _careerHistory = null!;
    private string? _firstName;
    private string? _lastName;
    private ProfileHeader _profileHeader = null!;
    private List<Project> _projects = [];
    private SkillSet _skillSet = null!;
    private List<Talent> _talents = [];
    private List<Testimonial> _testimonials = [];

    public ProfileEntityBuilder(IFixture fixture, JobMagnetDbContext context = null!)
    {
        _fixture = fixture;
        _clock = new DeterministicClock();
        _guidGenerator = new SequentialGuidGenerator();
    }


    public ProfileEntityBuilder WithResume()
    {
        _profileHeader = _fixture.Create<ProfileHeader>();
        return this;
    }

    public ProfileEntityBuilder WithContactInfo(ContactType[] availableContactTypes, int count = 5)
    {
        if (count > availableContactTypes.Length)
            throw new ArgumentOutOfRangeException(nameof(count), $"Count exceeds the number of available contact types. ({availableContactTypes.Length})");

        if (_profileHeader == null) throw new InvalidOperationException("Cannot add contact info without a profileHeader. Call WithProfileHeader() first.");

        while (_profileHeader.ContactInfo?.Count < count)
        {
            var contactType = Faker.PickRandom(availableContactTypes);
            if (_profileHeader.ContactInfo.Any(ci => ci.ContactType == contactType)) continue;

            var value = GenerateContactDetails(contactType.Name);
            _profileHeader.AddContactInfo(_guidGenerator, _clock, value, contactType);
        }

        return this;
    }

    public ProfileEntityBuilder WithSkillSet(bool loadSkill = true)
    {
        if (loadSkill)
            _skillSet = _fixture.Create<SkillSet>();

        return this;
    }

    public ProfileEntityBuilder WithSkills(SkillType[] availableSkillTypes, int count = 5)
    {
        if (count == 0)
            return this;

        if (count > availableSkillTypes.Length)
            throw new ArgumentOutOfRangeException(nameof(count), $"Count exceeds the number of available skill types. ({availableSkillTypes.Length})");

        if (_skillSet == null) throw new InvalidOperationException("Cannot add skills without a skill set. Call WithSkillSet() first.");

        while (_skillSet.Skills.Count < count)
        {
            var skillsToAdd = Faker.PickRandom(availableSkillTypes);
            if (_skillSet.SkillExists(skillsToAdd)) continue;

            var proficiencyLevel = (ushort)Faker.Random.Int(1, 10);
            _skillSet.AddSkill(_guidGenerator, _clock, proficiencyLevel, skillsToAdd);
        }

        return this;
    }

    public ProfileEntityBuilder WithTalents(int count = 5)
    {
        if (count >= StaticCustomizations.Talents.Length)
            throw new ArgumentOutOfRangeException(nameof(count), "Count exceeds the number of available talents.");

        var talents = new List<Talent>();

        while (talents.Count < count)
        {
            var talent = _fixture.Create<Talent>();

            if (talents.Any(t => t.Description == talent.Description))
                continue;

            talents.Add(talent);
        }

        _talents = talents;

        return this;
    }

    public ProfileEntityBuilder WithProjects(int count = 5)
    {
        _projects = _fixture.CreateMany<Project>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithSummary()
    {
        _careerHistory = _fixture.Create<CareerHistory>();
        return this;
    }

    public ProfileEntityBuilder WithSummary(CareerHistory careerHistory)
    {
        _careerHistory = careerHistory;
        return this;
    }

    public ProfileEntityBuilder WithEducation(int count = 5)
    {
        if (_careerHistory == null) throw new InvalidOperationException("Cannot add contact info without a careerHistory. Call WithCareerHistory() first.");

        foreach (var education in _fixture.CreateMany<Qualification>(count).ToList())
            _careerHistory.AddEducation(education);

        return this;
    }

    public ProfileEntityBuilder WithWorkExperience(int count = 5)
    {
        if (_careerHistory == null) throw new InvalidOperationException("Cannot add contact info without a careerHistory. Call WithCareerHistory() first.");

        foreach (var workExperience in _fixture.CreateMany<WorkExperience>(count).ToList())
            _careerHistory.AddWorkExperience(workExperience);

        return this;
    }

    public ProfileEntityBuilder WithTestimonials(int count = 5)
    {
        _testimonials = _fixture.CreateMany<Testimonial>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public ProfileEntityBuilder WithLastName(string? lastName)
    {
        _lastName = lastName;
        return this;
    }

    public Profile Build()
    {
        var profile = _fixture.Create<Profile>();

        if (_firstName is not null) profile.ChangeFirstName(_firstName);

        if (_lastName is not null) profile.ChangeLastName(_lastName);

        if (_profileHeader is not null) profile.AddResume(_profileHeader);

        if (_careerHistory is not null) profile.AddSummary(_careerHistory);

        if (_skillSet is not null) profile.AddSkillSet(_skillSet);

        LoadTestimonials(profile);
        LoadProjects(profile);
        LoadTalents(profile);

        return profile;
    }

    private void LoadTestimonials(Profile profile)
    {
        if (_testimonials.Count <= 0) return;

        foreach (var item in _testimonials)
            profile.AddTestimonial(
                _guidGenerator,
                item.Name,
                item.JobTitle,
                item.Feedback,
                item.PhotoUrl);
    }

    private void LoadProjects(Profile profile)
    {
        if (_projects.Count <= 0) return;

        foreach (var gallery in _projects)
            profile.AddProject(
                _guidGenerator,
                gallery.Title,
                gallery.Description,
                gallery.UrlLink,
                gallery.UrlImage,
                gallery.UrlVideo,
                gallery.Type);
    }

    private void LoadTalents(Profile profile)
    {
        if (_talents.Count <= 0) return;

        foreach (var talent in _talents)
            profile.TalentShowcase.AddTalent(talent.Description);
    }

    private static string GenerateContactDetails(string contactType)
    {
        return contactType switch
        {
            "Email" => Faker.Person.Email,
            "Phone" => Faker.Phone.PhoneNumber(),
            "LinkedIn" => $"https://linkedin.com/in/{Faker.Internet.UserName()}",
            "GitHub" => $"https://github.com/{Faker.Internet.UserName()}",
            "Website" => Faker.Internet.Url(),
            _ => Faker.Internet.DomainName()
        };
    }
}