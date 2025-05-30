using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileParseDtoBuilder(IFixture fixture)
{
    private List<PortfolioGalleryParseDto> _portfolio = [];
    private ResumeParseDto _resume = null!;
    private ServiceParseDto _services = null!;
    private SkillParseDto _skill = null!;
    private SummaryParseDto _summaries = null!;
    private List<TalentParseDto> _talents = [];
    private List<TestimonialParseDto> _testimonials = [];

    public ProfileParseDtoBuilder WithResume()
    {
        _resume = fixture.Create<ResumeParseDto>();
        return this;
    }

    public ProfileParseDtoBuilder WithSkills()
    {
        _skill = fixture.Create<SkillParseDto>();
        return this;
    }

    public ProfileParseDtoBuilder WithServices()
    {
        _services = fixture.Create<ServiceParseDto>();
        return this;
    }

    public ProfileParseDtoBuilder WithContactInfo(int count = 5)
    {
        _resume.ContactInfo = fixture.CreateMany<ContactInfoParseDto>(count).ToList();
        return this;
    }

    public ProfileParseDtoBuilder WithTalents(int count = 5)
    {
        _talents = fixture.CreateMany<TalentParseDto>(count).ToList();
        return this;
    }

    public ProfileParseDtoBuilder WithPortfolio(int count = 5)
    {
        _portfolio = fixture.CreateMany<PortfolioGalleryParseDto>(count).ToList();
        return this;
    }

    public ProfileParseDtoBuilder WithSummaries(int count = 5)
    {
        _summaries = fixture.Create<SummaryParseDto>();
        return this;
    }

    public ProfileParseDtoBuilder WithTestimonials(int count = 5)
    {
        _testimonials = fixture.CreateMany<TestimonialParseDto>(count).ToList();
        return this;
    }

    public ProfileParseDto Build()
    {
        var profile = fixture.Create<ProfileParseDto>();

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