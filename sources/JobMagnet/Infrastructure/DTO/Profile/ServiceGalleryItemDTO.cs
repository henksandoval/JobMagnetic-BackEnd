namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class ServiceGalleryItemDTO
{
    public int Position { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string UrlLink { get; set; }
    public string UrlImage { get; set; }
    public string UrlVideo { get; set; }
    public string Type { get; set; }
    public virtual ServiceDTO Service { get; set; }
}