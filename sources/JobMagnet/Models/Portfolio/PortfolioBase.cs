namespace JobMagnet.Models.Portfolio;

public abstract class PortfolioBase
{
    public required IList<PortfolioGalleryItemRequest> GalleryItems { get; set; }
}