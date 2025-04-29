using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Portfolio;

public sealed record PortfolioCreateCommand
{
    public PortfolioBase? PortfolioData { get; init; }
}