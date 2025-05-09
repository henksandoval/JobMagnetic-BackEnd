using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Summary;

public sealed record SummaryModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required SummaryBase SummaryData { get; init; }
}