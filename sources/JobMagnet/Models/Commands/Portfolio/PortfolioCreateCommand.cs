using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Portfolio;

public sealed class PortfolioCreateCommand : PortfolioBase
{
    public PortfolioBase? PortfolioData { get; init; }
}