namespace JobMagnet.Models.Portfolio;

public abstract class PortfolioBase
{
    public required IList<PortfolioGalleryItemCreateRequest> GalleryItems { get; set; }
}