namespace JobMagnet.Models.Portfolio;

public sealed class PortfolioCreateRequest : PortfolioBase
{
    public IList<PortfolioGalleryItemCreateRequest> GalleryItems { get; set; }
}