namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public record ServiceParseDto
{
    public ICollection<GalleryItemParseDto> GalleryItems { get; set; }
    public string? Overview { get; set; }
}