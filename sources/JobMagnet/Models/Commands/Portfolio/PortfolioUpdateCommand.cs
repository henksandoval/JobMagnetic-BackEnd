using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Portfolio;

public sealed class PortfolioUpdateCommand : PortfolioBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
    public PortfolioBase PortfolioData { get; set; }
}