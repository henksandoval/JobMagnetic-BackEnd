using JobMagnet.Models.Base;
using JobMagnet.Models.Commands.Summary.Education;
using JobMagnet.Models.Commands.Summary.WorkExperience;

namespace JobMagnet.Models.Commands.Summary;

public sealed class SummaryComplexRequest : SummaryBase
{
    public long? Id { get; init; }
    public IList<EducationRequest> Education { get; set; } = Enumerable.Empty<EducationRequest>().ToList();

    public IList<WorkExperienceRequest> WorkExperiences { get; set; } =
        Enumerable.Empty<WorkExperienceRequest>().ToList();
}