using AutoFixture;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class SummaryEntityBuilder(IFixture fixture)
{
    private List<Qualification> _education = [];
    private List<WorkExperience> _workExperiences = [];

    public SummaryEntityBuilder WithEducation(int count = 5)
    {
        _education = fixture.CreateMany<Qualification>(count).ToList();
        return this;
    }

    public SummaryEntityBuilder WithWorkExperiences(int count = 5)
    {
        _workExperiences = fixture.CreateMany<WorkExperience>(count).ToList();
        return this;
    }

    public CareerHistory Build()
    {
        var summary = fixture.Create<CareerHistory>();

        foreach (var education in _education)
            summary.AddEducation(education);

        foreach (var workExperience in _workExperiences)
            summary.AddWorkExperience(workExperience);

        return summary;
    }
}