namespace JobMagnet.Models.Portfolio;

public sealed class PortfolioCreateRequest : PortfolioBase
{
    public IList<PortfolioGalleryItemCreateRequest> PortfolioGalleryItems { get; set; }
}