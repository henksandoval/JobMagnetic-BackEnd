namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class GalleryItemParseDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? UrlLink { get; set; }
    public string? UrlImage { get; set; }
    public string? UrlVideo { get; set; }
    public string? Type { get; set; }
}