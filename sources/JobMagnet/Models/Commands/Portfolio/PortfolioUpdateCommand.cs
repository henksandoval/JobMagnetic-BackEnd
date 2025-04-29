using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Portfolio;

public sealed record PortfolioUpdateCommand : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required PortfolioBase PortfolioData { get; init; }
}