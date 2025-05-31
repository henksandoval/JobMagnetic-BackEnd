using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Portfolio;

public sealed record PortfolioCommand
{
    public PortfolioBase? PortfolioData { get; init; }
}