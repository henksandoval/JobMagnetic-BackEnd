using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Summary;

public sealed record SummaryResponse : IIdentifierBase<long>
{
    public required SummaryBase SummaryData { get; init; }
    public required long Id { get; init; }
}