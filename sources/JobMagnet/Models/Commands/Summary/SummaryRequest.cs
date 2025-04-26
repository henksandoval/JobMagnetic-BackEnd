using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Summary;

public sealed class SummaryRequest : SummaryBase
{
    public long? Id { get; init; }
}