using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ServiceEntity : SoftDeletableEntity<long>
{
    public required string Overview { get; set; }

    public virtual ICollection<ServiceGalleryItemEntity> GalleryItems { get; set; } =
        new HashSet<ServiceGalleryItemEntity>();
}