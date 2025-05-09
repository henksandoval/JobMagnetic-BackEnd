using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Summary;

public sealed record SummaryComplexCommand
{
    public long? Id { get; init; }
    public IList<EducationBase> Education { get; init; } = Enumerable.Empty<EducationBase>().ToList();

    public IList<WorkExperienceBase> WorkExperiences { get; init; } = Enumerable.Empty<WorkExperienceBase>().ToList();
}