using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder
{
    private readonly IClock _clock;
    private readonly IGuidGenerator _guidGenerator;
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private string? _firstName;
    private string? _lastName;
    private List<Project> _projects = [];
    private Headline _headline = null!;
    private SkillSet _skillSet = null!;
    private CareerHistory _careerHistory = null!;
    private List<Talent> _talents = [];
    private List<Testimonial> _testimonials = [];
    private readonly IFixture _fixture;

    public ProfileEntityBuilder(IFixture fixture)
    {
        _fixture = fixture;
        _clock = new DeterministicClock();
        _guidGenerator = new SequentialGuidGenerator();
    }


    public ProfileEntityBuilder WithResume()
    {
        _headline = _fixture.Create<Headline>();
        return this;
    }

    public ProfileEntityBuilder WithContactInfo(int count = 5)
    {
        if (count > ContactTypeSeeder.Count)
            throw new ArgumentOutOfRangeException(nameof(count), "Count exceeds the number of available contact types.");

        if (_headline == null) throw new InvalidOperationException("Cannot add contact info without a headline. Call WithResume() first.");

        var addedContactInfo = new Dictionary<string, ContactType>();

        while (_headline.ContactInfo?.Count < count)
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

            _headline.AddContactInfo(_guidGenerator, _clock, value, contactTypeToAdd);
        }

        return this;
    }

    public ProfileEntityBuilder WithSkillSet()
    {
        _skillSet = _fixture.Create<SkillSet>();
        return this;
    }

    public ProfileEntityBuilder WithSkills(int count = 5)
    {
        // if (count > SkillSeeder.Count)
        //     throw new ArgumentOutOfRangeException(nameof(count), "Count exceeds the number of available skill types.");

        if (_skillSet == null) throw new InvalidOperationException("Cannot add skills without a skill set. Call WithSkillSet() first.");

        var random = new Random();
        var addedSkillTypes = new HashSet<string>();
        var addedSkillCategories = new Dictionary<string, SkillCategory>();

        while (_skillSet.Skills.Count < count)
        {
            var skillType = _fixture.Create<SkillType>();

            if (addedSkillCategories.TryGetValue(skillType.Category.Name, out var existingCategory))
                skillType.SetCategory(existingCategory);
            else
                addedSkillCategories.Add(skillType.Category.Name, skillType.Category);

            var proficiencyLevel = (ushort)random.Next(1, 10);

            if (addedSkillTypes.Add(skillType.Name)) _skillSet.AddSkill(_guidGenerator, _clock, proficiencyLevel, skillType);
        }

        return this;
    }

    public ProfileEntityBuilder WithTalents(int count = 5)
    {
        while (_talents?.Count < count)
        {
            _talents = _fixture.CreateMany<Talent>(count).ToList();
        }
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
        if (_careerHistory == null) throw new InvalidOperationException("Cannot add contact info without a careerHistory. Call WithSummary() first.");

        foreach (var education in _fixture.CreateMany<Qualification>(count).ToList())
            _careerHistory.AddEducation(education);

        return this;
    }

    public ProfileEntityBuilder WithWorkExperience(int count = 5)
    {
        if (_careerHistory == null) throw new InvalidOperationException("Cannot add contact info without a careerHistory. Call WithSummary() first.");

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

        if (_headline is not null) profile.AddResume(_headline);

        if (_careerHistory is not null) profile.AddSummary(_careerHistory);

        if (_skillSet is not null) profile.AddSkill(_skillSet);

        if (_testimonials.Count > 0)
            foreach (var item in _testimonials)
                profile.SocialProof.AddTestimonial(
                    _guidGenerator,
                    _clock,
                    item.Name,
                    item.JobTitle,
                    item.Feedback,
                    item.PhotoUrl);

        if (_projects.Count > 0)
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

        if (_talents.Count > 0) return profile;
        foreach (var talent in _talents)
            profile.TalentShowcase.AddTalent(_guidGenerator, _clock, talent.Description);

        return profile;
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