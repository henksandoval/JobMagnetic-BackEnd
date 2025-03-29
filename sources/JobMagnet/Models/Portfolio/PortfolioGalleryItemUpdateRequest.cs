using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Portfolio;

public sealed class PortfolioGalleryItemUpdateRequest : PortfolioGalleryItemBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}