using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Summary;

public sealed class SummaryPatchCommand : SummaryBase
{
    public long? Id { get; init; }
}