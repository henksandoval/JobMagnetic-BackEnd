using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Summary;

public sealed record SummaryResponse : IIdentifierBase<Guid>
{
    public required SummaryBase SummaryData { get; init; }
    public required Guid Id { get; init; }
}