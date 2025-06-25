using AutoFixture;
using Bogus;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Contact;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder(IFixture fixture)
{
    private string? _firstName;
    private string? _lastName;
    private ResumeEntity _resume = null!;
    private SkillSet _skillSet = null!;
    private SummaryEntity _summary = null!;
    private List<Project> _projects = [];
    private List<TalentEntity> _talents = [];
    private List<TestimonialEntity> _testimonials = [];
    private static readonly Faker Faker = FixtureBuilder.Faker;


    public ProfileEntityBuilder WithResume()
    {
        _resume = fixture.Create<ResumeEntity>();
        return this;
    }

    public ProfileEntityBuilder WithContactInfo(int count = 5)
    {
        if (count > ContactTypeDataFactory.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count exceeds the number of available contact types.");
        }

        if (_resume == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a resume. Call WithResume() first.");
        }

        var addedContactInfo = new Dictionary<string, ContactType>();

        while (_resume.ContactInfo?.Count < count)
        {
            var requestedContactType = fixture.Create<ContactType>();
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

            _resume.AddContactInfo(value, contactTypeToAdd);
        }

        return this;
    }

    public ProfileEntityBuilder WithSkillSet()
    {
        _skillSet = fixture.Create<SkillSet>();
        return this;
    }

    public ProfileEntityBuilder WithSkills(int count = 5)
    {
        if (count > SkillDataFactory.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count exceeds the number of available skill types.");
        }

        if (_skillSet == null)
        {
            throw new InvalidOperationException("Cannot add skills without a skill set. Call WithSkillSet() first.");
        }

        var random = new Random();
        var addedSkillTypes = new HashSet<string>();
        var addedSkillCategories = new Dictionary<string, SkillCategory>();

        while (_skillSet.Skills.Count < count)
        {
            var skillType = fixture.Create<SkillType>();

            if (addedSkillCategories.TryGetValue(skillType.Category.Name, out var existingCategory))
            {
                skillType.SetCategory(existingCategory);
            }
            else
            {
                addedSkillCategories.Add(skillType.Category.Name, skillType.Category);
            }

            var proficiencyLevel = (ushort) random.Next(1, 10);

            if (addedSkillTypes.Add(skillType.Name))
            {
                _skillSet.AddSkill(proficiencyLevel, skillType);
            }
        }

        return this;
    }

    public ProfileEntityBuilder WithTalents(int count = 5)
    {
        _talents = fixture.CreateMany<TalentEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithProjects(int count = 5)
    {
        _projects = fixture.CreateMany<Project>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithSummary()
    {
        _summary = fixture.Create<SummaryEntity>();
        return this;
    }

    public ProfileEntityBuilder WithSummary(SummaryEntity summary)
    {
        _summary = summary;
        return this;
    }

    public ProfileEntityBuilder WithEducation(int count = 5)
    {
        if (_summary == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a summary. Call WithSummary() first.");
        }

        foreach (var education in fixture.CreateMany<EducationEntity>(count).ToList())
            _summary.AddEducation(education);

        return this;
    }

    public ProfileEntityBuilder WithWorkExperience(int count = 5)
    {
        if (_summary == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a summary. Call WithSummary() first.");
        }

        foreach (var workExperience in fixture.CreateMany<WorkExperienceEntity>(count).ToList())
            _summary.AddWorkExperience(workExperience);

        return this;
    }

    public ProfileEntityBuilder WithTestimonials(int count = 5)
    {
        _testimonials = fixture.CreateMany<TestimonialEntity>(count).ToList();
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

    public ProfileEntity Build()
    {
        var profile = fixture.Create<ProfileEntity>();

        if (_firstName is not null)
        {
            profile.FirstName = _firstName;
        }

        if (_lastName is not null)
        {
            profile.LastName = _lastName;
        }

        if (_resume is not null)
        {
            profile.AddResume(_resume);
        }

        if (_summary is not null)
        {
            profile.AddSummary(_summary);
        }

        if (_skillSet is not null)
        {
            profile.AddSkill(_skillSet);
        }

        if (_testimonials.Count > 0)
        {
            foreach (var item in _testimonials)
                profile.SocialProof.AddTestimonial(item.Name, item.JobTitle, item.Feedback, item.PhotoUrl);
        }

        if (_projects.Count > 0)
        {
            foreach (var gallery in _projects)
                profile.Portfolio.AddProject(
                    gallery.Title,
                    gallery.Description,
                    gallery.UrlLink,
                    gallery.UrlImage,
                    gallery.UrlVideo,
                    gallery.Type);
        }

        if (_talents.Count > 0)
        {
            profile.Talents = _talents;
        }

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