namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record GalleryProperties(
    string Title,
    string Description,
    string UrlLink = "",
    string UrlImage = "",
    string Type = "",
    string UrlVideo = ""
);