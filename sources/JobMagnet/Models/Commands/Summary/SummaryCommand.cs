using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Summary;

public sealed record SummaryCommand
{
    public required SummaryBase SummaryData { get; init; }
}