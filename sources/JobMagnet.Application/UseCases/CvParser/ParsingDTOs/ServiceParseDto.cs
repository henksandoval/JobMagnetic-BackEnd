using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public record ServiceParseDto : IParsedService
{
    public string? Overview { get; set; }
    public ICollection<GalleryItemParseDto> GalleryItemList { private get; set; } = new HashSet<GalleryItemParseDto>();

    IReadOnlyCollection<IParsedGalleryItem> IParsedService.GalleryItems =>
        new List<IParsedGalleryItem>(GalleryItemList);
}