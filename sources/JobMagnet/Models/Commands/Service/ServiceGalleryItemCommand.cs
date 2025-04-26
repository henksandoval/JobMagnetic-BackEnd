using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceGalleryItemCommand : ServiceItemBase
{
    public long? Id { get; set; }
}