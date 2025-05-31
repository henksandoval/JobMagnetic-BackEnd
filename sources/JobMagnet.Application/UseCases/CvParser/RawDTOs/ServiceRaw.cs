namespace JobMagnet.Application.UseCases.CvParser.RawDTOs;

public record ServiceRaw
{
    public string? Overview { get; set; }
    public ICollection<GalleryItemRaw> GalleryItems { get; set; }
}