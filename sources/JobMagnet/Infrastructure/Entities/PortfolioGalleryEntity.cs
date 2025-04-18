using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class PortfolioGalleryEntity : SoftDeletableEntity<long>
{
    public virtual ICollection<PortfolioGalleryItemEntityToRemove> GalleryItems { get; set; } =
        new HashSet<PortfolioGalleryItemEntityToRemove>();

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
}