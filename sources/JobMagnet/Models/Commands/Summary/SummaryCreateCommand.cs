using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Summary;

public sealed record SummaryCreateCommand
{
    public required SummaryBase SummaryData { get; init; }
}