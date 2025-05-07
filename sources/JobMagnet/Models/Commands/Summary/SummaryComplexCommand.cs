using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Summary;

public sealed class SummaryComplexCommand
{
    public long? Id { get; init; }
    public IList<EducationBase> Education { get; set; } = Enumerable.Empty<EducationBase>().ToList();

    public IList<WorkExperienceBase> WorkExperiences { get; set; } =
        Enumerable.Empty<WorkExperienceBase>().ToList();
}