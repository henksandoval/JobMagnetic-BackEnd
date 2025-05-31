using AutoFixture;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class SummaryEntityBuilder(IFixture fixture)
{
    private List<EducationEntity> _education = [];
    private List<WorkExperienceEntity> _workExperiences = [];

    public SummaryEntityBuilder WithEducation(int count = 5)
    {
        _education = fixture.CreateMany<EducationEntity>(count).ToList();
        return this;
    }

    public SummaryEntityBuilder WithWorkExperiences(int count = 5)
    {
        _workExperiences = fixture.CreateMany<WorkExperienceEntity>(count).ToList();
        return this;
    }

    public SummaryEntity Build()
    {
        var summary = fixture.Create<SummaryEntity>();
        summary.Education = _education;
        summary.WorkExperiences = _workExperiences;
        return summary;
    }
}