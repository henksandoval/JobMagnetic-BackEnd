using JobMagnet.Domain.Core.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public record ServiceParseDto : IParsedService
{
    public ICollection<GalleryItemParseDto> GalleryItems { get; set; }
    public string? Overview { get; set; }

    IReadOnlyCollection<IParsedGalleryItem> IParsedService.GalleryItems =>
        new List<IParsedGalleryItem>(GalleryItems).AsReadOnly();
}