using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Summary;

public sealed class SummaryModel : SummaryBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}