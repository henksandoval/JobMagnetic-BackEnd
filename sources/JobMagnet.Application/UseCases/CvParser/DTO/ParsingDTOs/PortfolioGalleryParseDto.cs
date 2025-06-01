namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class PortfolioGalleryParseDto
{
    public int Position { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? UrlLink { get; set; }
    public string? UrlImage { get; set; }
    public string? UrlVideo { get; set; }
    public string? Type { get; set; }
}