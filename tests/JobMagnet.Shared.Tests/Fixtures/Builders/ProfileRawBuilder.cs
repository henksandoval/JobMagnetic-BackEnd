using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileRawBuilder
{
    private readonly IFixture _fixture;
    private ProfileRaw _instance;

    public ProfileRawBuilder(IFixture fixture)
    {
        _fixture = fixture;
        _instance = fixture.Create<ProfileRaw>();
    }

    public ProfileRawBuilder WithBirthDate(string? rawDateString)
    {
        _instance = _instance with { BirthDate = rawDateString };
        return this;
    }

    public ProfileRawBuilder WithResume()
    {
        _instance = _instance with { Resume = _fixture.Create<ResumeRaw>() };
        return this;
    }

    public ProfileRawBuilder WithContactInfo(int count = 5)
    {
        if (_instance.Resume == null)
            throw new InvalidOperationException("Cannot add contact info without a resume. Call WithProfileHeader() first.");

        var contactInfo = _fixture.CreateMany<ContactInfoRaw>(count).ToList();
        return WithContactInfo(contactInfo);
    }

    public ProfileRawBuilder WithContactInfo(List<ContactInfoRaw> contactInfo)
    {
        var resumeBase = _instance.Resume ??
                         throw new InvalidOperationException("ProfileHeader must be set before adding contact info. Call WithProfileHeader() first.");
        var updatedResume = resumeBase with { ContactInfo = contactInfo };

        _instance = _instance with { Resume = updatedResume };

        return this;
    }

    public ProfileRawBuilder WithSkillSet()
    {
        var skillSet = _fixture.Create<SkillSetRaw>();
        _instance = _instance with { SkillSet = skillSet };
        return this;
    }

    public ProfileRawBuilder WithSkills(List<SkillRaw> skills)
    {
        var skillSet = _instance.SkillSet ??
                       throw new InvalidOperationException("SkillSet must be set before adding contact info. Call WithSkillSet() first.");
        var skillSetRaw = skillSet with { Skills = skills };

        _instance = _instance with { SkillSet = skillSetRaw };

        return this;
    }

    public ProfileRawBuilder WithServices()
    {
        var services = _fixture.Create<ServiceRaw>();
        _instance = _instance with { Services = services };
        return this;
    }

    public ProfileRawBuilder WithTalents(int count = 5)
    {
        var talents = _fixture.CreateMany<TalentRaw>(count).ToList();
        _instance = _instance with { Talents = talents };
        return this;
    }

    public ProfileRawBuilder WithProjects(int count = 5)
    {
        var project = _fixture.CreateMany<ProjectRaw>(count).ToList();
        _instance = _instance with { Project = project };
        return this;
    }

    public ProfileRawBuilder WithSummaries()
    {
        var summary = _fixture.Create<SummaryRaw>();
        _instance = _instance with { Summary = summary };
        return this;
    }

    public ProfileRawBuilder WithEducation(int count = 5)
    {
        var educationList = _fixture.CreateMany<EducationRaw>(count).ToList();
        var summaryBase = _instance.Summary ?? new SummaryRaw(null, [], []);
        _instance = _instance with { Summary = summaryBase with { Education = educationList } };
        return this;
    }

    public ProfileRawBuilder WithWorkExperience(int count = 5)
    {
        var workExperienceList = _fixture.CreateMany<WorkExperienceRaw>(count).ToList();
        var summaryBase = _instance.Summary ?? new SummaryRaw(null, [], []);
        _instance = _instance with { Summary = summaryBase with { WorkExperiences = workExperienceList } };
        return this;
    }

    public ProfileRawBuilder WithTestimonials(int count = 5)
    {
        var testimonials = _fixture.CreateMany<TestimonialRaw>(count).ToList();
        _instance = _instance with { Testimonials = testimonials };
        return this;
    }

    public ProfileRaw Build() => _instance;
}