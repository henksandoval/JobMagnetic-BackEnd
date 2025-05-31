using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileRawBuilder(IFixture fixture)
{
    private readonly ProfileRaw _instance = fixture.Create<ProfileRaw>();

    public ProfileRawBuilder WithBirthDate(string? rawDateString)
    {
        _instance.BirthDate = rawDateString;
        return this;
    }

    public ProfileRawBuilder WithResume()
    {
        _instance.Resume = fixture.Create<ResumeRaw>();
        return this;
    }

    public ProfileRawBuilder WithSkills()
    {
        _instance.Skill = fixture.Create<SkillRaw>();
        return this;
    }

    public ProfileRawBuilder WithServices()
    {
        _instance.Services = fixture.Create<ServiceRaw>();
        return this;
    }

    public ProfileRawBuilder WithContactInfo(int count = 5)
    {
        _instance.Resume ??= new ResumeRaw();
        _instance.Resume!.ContactInfo = fixture.CreateMany<ContactInfoRaw>(count).ToList();
        return this;
    }

    public ProfileRawBuilder WithTalents(int count = 5)
    {
        _instance.Talents = fixture.CreateMany<TalentRaw>(count).ToList();
        return this;
    }

    public ProfileRawBuilder WithPortfolio(int count = 5)
    {
        _instance.PortfolioGallery = fixture.CreateMany<PortfolioGalleryRaw>(count).ToList();
        return this;
    }

    public ProfileRawBuilder WithSummaries(int count = 5)
    {
        _instance.Summary = fixture.Create<SummaryRaw>();
        return this;
    }

    public ProfileRawBuilder WithTestimonials(int count = 5)
    {
        _instance.Testimonials = fixture.CreateMany<TestimonialRaw>(count).ToList();
        return this;
    }

    public ProfileRaw Build()
    {
        return _instance;
    }
}