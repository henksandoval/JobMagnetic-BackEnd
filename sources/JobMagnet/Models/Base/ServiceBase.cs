namespace JobMagnet.Models.Base;

public sealed record ServiceBase
{
    public long ProfileId { get; set; }
    public string? Overview { get; set; }
    public IList<ServiceGalleryItemBase> GalleryItems { get; set; } = Enumerable.Empty<ServiceGalleryItemBase>().ToList();
}