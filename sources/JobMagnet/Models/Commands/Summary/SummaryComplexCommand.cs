using JobMagnet.Models.Base;
using JobMagnet.Models.Commands.Summary.WorkExperience;

namespace JobMagnet.Models.Commands.Summary;

public sealed class SummaryComplexCommand
{
    public long? Id { get; init; }
    public IList<EducationBase> Education { get; set; } = Enumerable.Empty<EducationBase>().ToList();

    public IList<WorkExperienceCommand> WorkExperiences { get; set; } =
        Enumerable.Empty<WorkExperienceCommand>().ToList();
}