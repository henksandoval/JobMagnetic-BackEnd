using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Domain.Entities.Base;

namespace JobMagnet.Domain.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class PortfolioGalleryEntity : SoftDeletableEntity<long>
{
    public int Position { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string UrlLink { get; set; }
    public string UrlImage { get; set; }
    public string UrlVideo { get; set; }
    public string Type { get; set; }

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
}