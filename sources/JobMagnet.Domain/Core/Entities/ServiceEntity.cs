using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ServiceEntity : SoftDeletableEntity<long>
{
    public required string Overview { get; set; }

    public virtual ICollection<ServiceGalleryItemEntity> GalleryItems { get; set; } =
        new HashSet<ServiceGalleryItemEntity>();

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
}