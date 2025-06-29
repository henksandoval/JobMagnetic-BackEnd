using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.CareerHistory;

public sealed record CareerHistoryCommand
{
    public required CareerHistoryBase CareerHistoryData { get; init; }
}