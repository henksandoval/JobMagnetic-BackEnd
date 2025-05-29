namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedGalleryItem
{
    string? Title { get; }
    string? Description { get; }
    string? Url { get; }
}