using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Portfolio;

public sealed class PortfolioUpdateRequest : PortfolioBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}