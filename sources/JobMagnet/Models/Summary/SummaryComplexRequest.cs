using JobMagnet.Models.Summary.Education;
using JobMagnet.Models.Summary.WorkExperience;

namespace JobMagnet.Models.Summary;

public sealed class SummaryComplexRequest : SummaryBase
{
    public long? Id { get; init; }
    public IList<EducationRequest> Education { get; set; } = Enumerable.Empty<EducationRequest>().ToList();
    public IList<WorkExperienceRequest> WorkExperiences { get; set; } = Enumerable.Empty<WorkExperienceRequest>().ToList();
}