using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class PortfolioEntity : SoftDeletableEntity<long>
{
    public virtual ICollection<PortfolioGalleryItemEntity> GalleryItems { get; set; } =
        new HashSet<PortfolioGalleryItemEntity>();

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
}