using JobMagnet.Models.Summary.Education;

namespace JobMagnet.Models.Summary;

public sealed class SummaryComplexRequest : SummaryBase
{
    public long? Id { get; init; }
    public IList<EducationRequest> Education { get; set; } = Enumerable.Empty<EducationRequest>().ToList();
}