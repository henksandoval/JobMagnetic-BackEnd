namespace JobMagnet.Models.Service;

public abstract class ServiceBase
{
    public required long ProfileId { get; set; }

    public string Overview { get; set; }
    public required IList<ServiceGalleryItemRequest> GalleryItems { get; set; }
}