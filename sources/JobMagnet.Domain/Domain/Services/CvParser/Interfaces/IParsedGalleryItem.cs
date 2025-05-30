namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedGalleryItem
{
    string? Title { get; }
    string? Description { get; }
    string? UrlLink { get; }
    string? UrlImage { get; }
    string? UrlVideo { get; }
    string? Type { get; }
}