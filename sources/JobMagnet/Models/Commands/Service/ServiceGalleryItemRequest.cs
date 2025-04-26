using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceGalleryItemRequest : ServiceItemBase
{
    public long? Id { get; set; }
}