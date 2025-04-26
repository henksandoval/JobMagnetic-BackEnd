using JobMagnet.Models.Base;

namespace JobMagnet.Models.Summary;

public sealed class SummaryRequest : SummaryBase
{
    public long? Id { get; init; }
}