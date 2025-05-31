using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Portfolio;

public sealed record PortfolioModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required PortfolioBase PortfolioData { get; init; }
}