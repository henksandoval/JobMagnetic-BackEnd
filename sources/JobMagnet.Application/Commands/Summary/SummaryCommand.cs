using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Commands.Summary;

public sealed record SummaryCommand
{
    public required SummaryBase SummaryData { get; init; }
}