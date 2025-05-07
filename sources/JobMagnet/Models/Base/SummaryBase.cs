using JobMagnet.Models.Commands.Summary.Education;
using JobMagnet.Models.Commands.Summary.WorkExperience;

namespace JobMagnet.Models.Base;

public class SummaryBase
{
    public required long ProfileId { get; set; }
    public required string? Introduction { get; set; }
    public required IList<EducationCommand> Education { get; set; } = Enumerable.Empty<EducationCommand>().ToList();

    public required IList<WorkExperienceCommand> WorkExperiences { get; set; } =
        Enumerable.Empty<WorkExperienceCommand>().ToList();
}