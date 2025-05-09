namespace JobMagnet.Models.Base;

public sealed record ServiceGalleryItemBase
{
    public long Id { get; set; }
    public int Position { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? UrlLink { get; set; }
    public string? UrlImage { get; set; }
    public string? UrlVideo { get; set; }
    public string? Type { get; set; }
}