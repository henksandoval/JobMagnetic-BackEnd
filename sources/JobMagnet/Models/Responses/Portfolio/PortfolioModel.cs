using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Portfolio;

public sealed class PortfolioModel : PortfolioBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}