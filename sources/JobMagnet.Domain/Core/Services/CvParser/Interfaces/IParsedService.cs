namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedService
{
    string? Overview { get; }
    IReadOnlyCollection<IParsedGalleryItem> GalleryItems { get; }
}