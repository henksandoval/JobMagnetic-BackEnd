using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Portfolio;

public sealed class PortfolioModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required PortfolioBase PortfolioData { get; init; }
}