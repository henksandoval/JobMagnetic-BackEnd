using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder(IFixture fixture)
{
    private ResumeEntity _resume = null!;
    private SkillEntity _skill = null!;
    private List<TalentEntity> _talents = [];
    private List<PortfolioGalleryEntity> _portfolio = [];
    private List<SummaryEntity> _summaries = [];
    private List<ServiceEntity> _services = [];
    private List<TestimonialEntity> _testimonials = [];

    public ProfileEntityBuilder WithResume()
    {
        _resume = fixture.Create<ResumeEntity>();
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
        _summaries = fixture.CreateMany<SummaryEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithServices(int count = 5)
    {
        _services = fixture.CreateMany<ServiceEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithSkills()
    {
        _skill = fixture.Create<SkillEntity>();
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
        profile.Portfolios = _portfolio;
        profile.Summaries = _summaries;
        profile.Services = _services;
        profile.Skill = _skill;
        profile.Testimonials = _testimonials;

        return profile;
    }
}