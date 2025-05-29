using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Commands.Portfolio;

public sealed record PortfolioCommand
{
    public PortfolioBase? PortfolioData { get; init; }
}