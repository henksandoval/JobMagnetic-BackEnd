using JobMagnet.Models.Commands.Service;

namespace JobMagnet.Models.Base;

public class ServiceBase
{
    public required long ProfileId { get; set; }

    public string Overview { get; set; }
    public required IList<ServiceGalleryItemCommand> GalleryItems { get; set; }
}