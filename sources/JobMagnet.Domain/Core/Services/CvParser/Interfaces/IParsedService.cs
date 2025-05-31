namespace JobMagnet.Domain.Core.Services.CvParser.Interfaces;

public interface IParsedService
{
    string? Overview { get; }
    IReadOnlyCollection<IParsedGalleryItem> GalleryItems { get; }
}