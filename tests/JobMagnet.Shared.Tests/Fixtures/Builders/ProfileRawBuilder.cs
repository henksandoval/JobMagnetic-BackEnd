using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileRawBuilder(IFixture fixture)
{
    private ProfileRaw _instance = fixture.Create<ProfileRaw>();

    public ProfileRawBuilder WithBirthDate(string? rawDateString)
    {
        _instance = _instance with { BirthDate = rawDateString };
        return this;
    }

    public ProfileRawBuilder WithResume()
    {
        _instance = _instance with { Resume = fixture.Create<ResumeRaw>() };
        return this;
    }

    public ProfileRawBuilder WithContactInfo(int count = 5)
    {
        var contactInfo = fixture.CreateMany<ContactInfoRaw>(count).ToList();
        return WithContactInfo(contactInfo);
    }

    public ProfileRawBuilder WithContactInfo(List<ContactInfoRaw> contactInfo)
    {
        var resumeBase = _instance.Resume ??
                         throw new InvalidOperationException("Resume must be set before adding contact info.");
        var updatedResume = resumeBase with { ContactInfo = contactInfo };
        _instance = _instance with { Resume = updatedResume };
        return this;
    }

    public ProfileRawBuilder WithSkills()
    {
        var skill = fixture.Create<SkillRaw>();
        _instance = _instance with { Skill = skill };
        return this;
    }

    public ProfileRawBuilder WithServices()
    {
        var services = fixture.Create<ServiceRaw>();
        _instance = _instance with { Services = services };
        return this;
    }

    public ProfileRawBuilder WithTalents(int count = 5)
    {
        var talents = fixture.CreateMany<TalentRaw>(count).ToList();
        _instance = _instance with { Talents = talents };
        return this;
    }

    public ProfileRawBuilder WithPortfolio(int count = 5)
    {
        var portfolioGallery = fixture.CreateMany<PortfolioGalleryRaw>(count).ToList();
        _instance = _instance with { PortfolioGallery = portfolioGallery };
        return this;
    }

    public ProfileRawBuilder WithSummaries()
    {
        var summary = fixture.Create<SummaryRaw>();
        _instance = _instance with { Summary = summary };
        return this;
    }

    public ProfileRawBuilder WithEducation(int count = 5)
    {
        var educationList = fixture.CreateMany<EducationRaw>(count).ToList();
        var summaryBase = _instance.Summary ?? new SummaryRaw(null, [], []);
        _instance = _instance with { Summary = summaryBase with { Education = educationList } };
        return this;
    }

    public ProfileRawBuilder WithWorkExperience(int count = 5)
    {
        var workExperienceList = fixture.CreateMany<WorkExperienceRaw>(count).ToList();
        var summaryBase = _instance.Summary ?? new SummaryRaw(null, [], []);
        _instance = _instance with { Summary = summaryBase with { WorkExperiences = workExperienceList } };
        return this;
    }

    public ProfileRawBuilder WithTestimonials(int count = 5)
    {
        var testimonials = fixture.CreateMany<TestimonialRaw>(count).ToList();
        _instance = _instance with { Testimonials = testimonials };
        return this;
    }

    public ProfileRaw Build()
    {
        return _instance;
    }
}