using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Summary;

public sealed class SummaryModel : SummaryBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}