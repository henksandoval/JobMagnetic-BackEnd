namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record ServiceRaw(string? Overview, ICollection<GalleryItemRaw>? GalleryItems);