using JobMagnet.Models.Base;

namespace JobMagnet.Models.Portfolio;

public sealed class PortfolioModel : PortfolioBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}