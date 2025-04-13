namespace JobMagnet.Models.Portfolio;

public abstract class PortfolioBase
{
    public required long ProfileId { get; set; }
    public required IList<PortfolioGalleryItemRequest> GalleryItems { get; set; }
}