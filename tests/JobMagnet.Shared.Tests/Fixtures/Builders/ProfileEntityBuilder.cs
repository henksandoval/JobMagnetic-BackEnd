using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Data;
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

    public ProfileEntityBuilder WithContactInfo(int count = 5)
    {
        if (count > ContactTypeSeeder.Count)
            throw new ArgumentOutOfRangeException(nameof(count), "Count exceeds the number of available contact types.");

        if (_profileHeader == null) throw new InvalidOperationException("Cannot add contact info without a profileHeader. Call WithProfileHeader() first.");

        var addedContactInfo = new Dictionary<string, ContactType>();

        while (_profileHeader.ContactInfo?.Count < count)
        {
            var requestedContactType = _fixture.Create<ContactType>();
            ContactType contactTypeToAdd;

            if (addedContactInfo.TryGetValue(requestedContactType.Name, out var existingContactType))
            {
                contactTypeToAdd = existingContactType;
            }
            else
            {
                addedContactInfo.Add(requestedContactType.Name, requestedContactType);
                contactTypeToAdd = requestedContactType;
            }

            var value = GenerateContactDetails(contactTypeToAdd.Name);

            _profileHeader.AddContactInfo(_guidGenerator, _clock, value, contactTypeToAdd);
        }

        return this;
    }

    public ProfileEntityBuilder WithSkillSet(bool loadSkill = true)
    {
        if (loadSkill)
            _skillSet = _fixture.Create<SkillSet>();

        return this;
    }

    public ProfileEntityBuilder WithSkills(int count = 5)
    {
        if (count == 0)
            return this;

        if (count > SkillRawData.Count)
            throw new ArgumentOutOfRangeException(nameof(count), $"Count exceeds the number of available skill types. ({SkillRawData.Count})");

        if (_skillSet == null) throw new InvalidOperationException("Cannot add skills without a skill set. Call WithSkillSet() first.");

        var typesWithCategory = SkillSeeder.SeedData.Types
            .Join(SkillSeeder.SeedData.Categories,
                type => type.CategoryId,
                cat => cat.Id,
                (type, cat) => (Type: type, Category: cat)
            ).ToList();

        while (_skillSet.Skills.Count < count)
        {
            var proposalSkill = Faker.PickRandom(typesWithCategory);
            if (_skillSet.Skills.Any(s => s.SkillTypeId == proposalSkill.Type.Id)) continue;

            var category = SkillCategory.Reconstitute(
                proposalSkill.Category.Id,
                _clock,
                proposalSkill.Category.Name
            );

            var skillType = SkillType.Reconstitute(
                proposalSkill.Type.Id,
                _clock,
                proposalSkill.Type.Name,
                category,
                proposalSkill.Type.IconUrl
            );

            var proficiencyLevel = (ushort)Faker.Random.Int(1, 10);
            _skillSet.AddSkill(_guidGenerator, _clock, proficiencyLevel, skillType);
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

        if (_skillSet is not null) profile.AddSkill(_skillSet);

        LoadTestimonials(profile);
        LoadProjects(profile);
        LoadTalents(profile);

        return profile;
    }

    private void LoadTestimonials(Profile profile)
    {
        if (_testimonials.Count <= 0) return;

        foreach (var item in _testimonials)
            profile.SocialProof.AddTestimonial(
                _guidGenerator,
                _clock,
                item.Name,
                item.JobTitle,
                item.Feedback,
                item.PhotoUrl);
    }

    private void LoadProjects(Profile profile)
    {
        if (_projects.Count <= 0) return;

        foreach (var gallery in _projects)
            profile.Portfolio.AddProject(
                _guidGenerator,
                _clock,
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