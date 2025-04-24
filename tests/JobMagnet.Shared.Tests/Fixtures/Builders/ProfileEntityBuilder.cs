using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder(IFixture fixture)
{
    private ResumeEntity _resume = null!;
    private SkillEntity _skill = null!;
    private ServiceEntity _services = null!;
    private SummaryEntity _summaries = null!;
    private List<TalentEntity> _talents = [];
    private List<PortfolioGalleryEntity> _portfolio = [];
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

    public ProfileEntityBuilder WithSummaries(int count = 5)
    {
        _summaries = fixture.Create<SummaryEntity>();
        return this;
    }

    public ProfileEntityBuilder WithTestimonials(int count = 5)
    {
        _testimonials = fixture.CreateMany<TestimonialEntity>(count).ToList();
        return this;
    }

    public ProfileEntity Build()
    {
        var profile = fixture.Create<ProfileEntity>();

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