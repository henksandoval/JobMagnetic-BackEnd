namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record PortfolioGalleryRaw(
    string? Title,
    string? Description,
    string? UrlLink,
    string? UrlImage,
    string? UrlVideo,
    string? Type);