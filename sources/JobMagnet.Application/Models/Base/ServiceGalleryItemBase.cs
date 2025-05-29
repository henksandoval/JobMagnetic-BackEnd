namespace JobMagnet.Application.Models.Base;

public sealed record ServiceGalleryItemBase
{
    public long Id { get; init; }
    public int Position { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? UrlLink { get; init; }
    public string? UrlImage { get; init; }
    public string? UrlVideo { get; init; }
    public string? Type { get; init; }
}