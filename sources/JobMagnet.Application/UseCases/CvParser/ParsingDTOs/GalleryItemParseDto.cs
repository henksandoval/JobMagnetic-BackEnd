using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class GalleryItemParseDto : IParsedGalleryItem
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? UrlLink { get; set; }
    public string? UrlImage { get; set; }
    public string? UrlVideo { get; set; }
    public string? Type { get; set; }
}