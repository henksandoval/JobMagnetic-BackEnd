using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Portfolio;

public sealed record PortfolioCommand
{
    public PortfolioBase? PortfolioData { get; init; }
}