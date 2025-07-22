using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.CareerHistory;

public sealed record CareerHistoryResponse : IIdentifierBase<Guid>
{
    public required CareerHistoryBase CareerHistoryData { get; init; }
    public required Guid Id { get; init; }
}