using JobMagnet.Models.Base;

namespace JobMagnet.Models.Service;

public sealed class ServiceGalleryItemRequest : ServiceItemBase
{
    public long? Id { get; set; }
}