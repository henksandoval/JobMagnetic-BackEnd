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
        IPostprocessComposer<ProfileEntity> composer = fixture.Build<ProfileEntity>();

        if (_firstName is not null)
        {
            composer = composer.With(p => p.FirstName, _firstName);
        }

        if (_lastName is not null)
        {
            composer = composer.With(p => p.LastName, _lastName);
        }

        var profile = composer.Create();

        profile.Resume = _resume;
        profile.Talents = _talents;
        profile.Services = _services;
        profile.PortfolioGallery = _portfolio;
        profile.Summary = _summaries;
        profile.Skill = _skill;
        profile.Testimonials = _testimonials;

        return profile;
    }
}