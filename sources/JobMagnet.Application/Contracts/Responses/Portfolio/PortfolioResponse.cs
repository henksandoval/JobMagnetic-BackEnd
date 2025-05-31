using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Portfolio;

public sealed record PortfolioResponse : IIdentifierBase<long>
{
    public required PortfolioBase PortfolioData { get; init; }
    public required long Id { get; init; }
}