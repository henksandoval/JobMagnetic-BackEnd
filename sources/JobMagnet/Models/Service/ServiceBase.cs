namespace JobMagnet.Models.Service;

public abstract class ServiceBase
{
    public string Overview { get; set; }
    public required IList<ServiceGalleryItemRequest> GalleryItems { get; set; }
}