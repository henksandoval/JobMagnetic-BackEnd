using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Summary;

public sealed record SummaryUpdateCommand
{
    public required long Id { get; init; }
    public required SummaryBase SummaryData { get; init; }
}