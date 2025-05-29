using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class GalleryItemParseDto : IParsedGalleryItem
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
}