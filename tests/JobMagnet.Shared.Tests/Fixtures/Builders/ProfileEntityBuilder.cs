using AutoFixture;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder(IFixture fixture)
{
    private string? _firstName;
    private string? _lastName;
    private ResumeEntity _resume = null!;
    private ServiceEntity _services = null!;
    private SkillSetEntity _skillSet = null!;
    private SummaryEntity _summary = null!;
    private List<PortfolioGalleryEntity> _portfolio = [];
    private List<TalentEntity> _talents = [];
    private List<TestimonialEntity> _testimonials = [];

    public ProfileEntityBuilder WithResume()
    {
        _resume = fixture.Create<ResumeEntity>();
        return this;
    }

    public ProfileEntityBuilder WithContactInfo(int count = 5)
    {
        if (_resume == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a resume. Call WithResume() first.");
        }

        fixture.Inject(_resume);

        var contactInfoCollection = fixture.CreateMany<ContactInfoEntity>(count).ToList();

        foreach (var contactInfo in contactInfoCollection)
        {
            _resume.AddContactInfo(contactInfo.Value, contactInfo.ContactType);
        }

        return this;
    }

    public ProfileEntityBuilder WithSkillSet()
    {
        _skillSet = fixture.Create<SkillSetEntity>();
        return this;
    }

    public ProfileEntityBuilder WithSkills(int count = 5)
    {
        if (_skillSet == null)
        {
            throw new InvalidOperationException("Cannot add skills without a skill set. Call WithSkillSet() first.");
        }

        var random = new Random();

        while (_skillSet.Skills.Count < count)
        {
            var skillType = fixture.Create<SkillType>();
            var proficiencyLevel = (ushort) random.Next(1, 10);

            if (_skillSet.SkillExists(skillType.Name))
            {
                continue;
            }

            _skillSet.AddSkill(proficiencyLevel, skillType);
        }

        return this;
    }

    public ProfileEntityBuilder WithServices()
    {
        _services = fixture.Create<ServiceEntity>();
        return this;
    }

    public ProfileEntityBuilder WithTalents(int count = 5)
    {
        _talents = fixture.CreateMany<TalentEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithPortfolio(int count = 5)
    {
        _portfolio = fixture.CreateMany<PortfolioGalleryEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithSummary()
    {
        _summary = fixture.Create<SummaryEntity>();
        return this;
    }

    public ProfileEntityBuilder WithEducation(int count = 5)
    {
        if (_summary == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a summary. Call WithSummary() first.");
        }

        _summary.Education = fixture.CreateMany<EducationEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithWorkExperience(int count = 5)
    {
        if (_summary == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a summary. Call WithSummary() first.");
        }

        _summary.WorkExperiences = fixture.CreateMany<WorkExperienceEntity>(count).ToList();
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
            profile.Resume = _resume;
        }

        if (_services is not null)
        {
            profile.Services = _services;
        }

        if (_summary is not null)
        {
            profile.Summary = _summary;
        }

        if (_skillSet is not null)
        {
            profile.SkillSet = _skillSet;
        }

        if (_testimonials.Count > 0)
        {
            profile.Testimonials = _testimonials;
        }

        if (_portfolio.Count > 0)
        {
            profile.PortfolioGallery = _portfolio;
        }

        if (_talents.Count > 0)
        {
            profile.Talents = _talents;
        }

        return profile;
    }
}