using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Summary;

public sealed class SummaryModel : SummaryBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}