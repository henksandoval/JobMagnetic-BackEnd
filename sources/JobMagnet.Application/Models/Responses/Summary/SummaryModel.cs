using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Models.Responses.Summary;

public sealed record SummaryModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required SummaryBase SummaryData { get; init; }
}