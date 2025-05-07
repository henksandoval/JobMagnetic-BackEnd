using JobMagnet.Models.Commands.Summary.Education;
using JobMagnet.Models.Commands.Summary.WorkExperience;

namespace JobMagnet.Models.Commands.Summary;

public sealed class SummaryComplexCommand
{
    public long? Id { get; init; }
    public IList<EducationCommand> Education { get; set; } = Enumerable.Empty<EducationCommand>().ToList();

    public IList<WorkExperienceCommand> WorkExperiences { get; set; } =
        Enumerable.Empty<WorkExperienceCommand>().ToList();
}