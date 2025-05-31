using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Summary;

public sealed record SummaryCommand
{
    public required SummaryBase SummaryData { get; init; }
}