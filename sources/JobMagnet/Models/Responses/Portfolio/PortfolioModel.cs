using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Portfolio;

public sealed class PortfolioModel : PortfolioBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
    public PortfolioBase? PortfolioData { get; init; }
}