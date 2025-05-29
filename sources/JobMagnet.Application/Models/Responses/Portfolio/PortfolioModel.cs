using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Models.Responses.Portfolio;

public sealed record PortfolioModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required PortfolioBase PortfolioData { get; init; }
}