using AutoFixture;
using AutoFixture.Dsl;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder(IFixture fixture)
{
    private string? _firstName;
    private string? _lastName;
    private ResumeEntity _resume = null!;
    private ServiceEntity _services = null!;
    private SkillEntity _skill = null!;
    private SummaryEntity _summaries = null!;
    private List<PortfolioGalleryEntity> _portfolio = [];
    private List<TalentEntity> _talents = [];
    private List<TestimonialEntity> _testimonials = [];

    public ProfileEntityBuilder WithResume()
    {
        _resume = fixture.Create<ResumeEntity>();
        return this;
    }

    public ProfileEntityBuilder WithSkills()
    {
        _skill = fixture.Create<SkillEntity>();
        return this;
    }

    public ProfileEntityBuilder WithServices()
    {
        _services = fixture.Create<ServiceEntity>();
        return this;
    }

    public ProfileEntityBuilder WithContactInfo(int count = 5)
    {
        if (_resume == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a resume. Call WithResume() first.");
        }

        _resume.ContactInfo = fixture.CreateMany<ContactInfoEntity>(count).ToList();
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

    public ProfileEntityBuilder WithSummaries()
    {
        _summaries = fixture.Create<SummaryEntity>();
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

        if (_summaries is not null)
        {
            profile.Summary = _summaries;
        }

        if (_skill is not null)
        {
            profile.Skill = _skill;
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