using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class PortfolioGalleryItemEntity : TrackableEntity<long>
{
    public int Position { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string UrlLink { get; set; }
    public string UrlImage { get; set; }
    public string UrlVideo { get; set; }
    public string Type { get; set; }

    [ForeignKey(nameof(Porfolio))] public long PorfolioId { get; set; }

    public virtual PortfolioEntity Porfolio { get; set; }
}