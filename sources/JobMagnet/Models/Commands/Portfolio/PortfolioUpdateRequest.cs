using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Portfolio;

public sealed class PortfolioUpdateRequest : PortfolioBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}