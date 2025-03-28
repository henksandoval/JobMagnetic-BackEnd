using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Portfolio;

public sealed class PortfolioModel : PortfolioBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}