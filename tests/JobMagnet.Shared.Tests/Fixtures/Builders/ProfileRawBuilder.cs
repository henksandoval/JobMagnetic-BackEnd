using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

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

    public ProfileRawBuilder WithContactInfo(int count = 5)
    {
        var contactInfo = fixture.CreateMany<ContactInfoRaw>(count).ToList();
        _instance.Resume = _instance.Resume! with { ContactInfo = contactInfo };
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

    public ProfileRawBuilder WithSummaries()
    {
        _instance.Summary = fixture.Create<SummaryRaw>();
        return this;
    }

    public ProfileRawBuilder WithEducation(int count = 5)
    {
        var educationList = fixture.CreateMany<EducationRaw>(count).ToList();
        _instance.Summary ??= new SummaryRaw("", [], []);
        _instance.Summary = _instance.Summary with { Education = educationList };
        return this;
    }

    public ProfileRawBuilder WithWorkExperience(int count = 5)
    {
        var workExperienceList = fixture.CreateMany<WorkExperienceRaw>(count).ToList();
        _instance.Summary ??= new SummaryRaw("", [], []);
        _instance.Summary = _instance.Summary with { WorkExperiences = workExperienceList };
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