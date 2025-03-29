using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ServiceGalleryItemEntity : TrackableEntity<long>
{
    public int Position { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string UrlLink { get; set; }
    public string UrlImage { get; set; }
    public string UrlVideo { get; set; }
    public string Type { get; set; }

    [ForeignKey(nameof(Service))] public long ServiceId { get; set; }

    public virtual ServiceEntity Service { get; set; }
}