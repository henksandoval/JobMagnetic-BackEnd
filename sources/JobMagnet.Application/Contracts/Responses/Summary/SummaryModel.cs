using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Summary;

public sealed record SummaryModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required SummaryBase SummaryData { get; init; }
}