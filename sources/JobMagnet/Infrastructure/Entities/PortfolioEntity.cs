using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class PortfolioEntity : SoftDeletableEntity<long>
{
    public virtual ICollection<PortfolioGalleryItemEntity> GalleryItems { get; set; } =
        new HashSet<PortfolioGalleryItemEntity>();
}