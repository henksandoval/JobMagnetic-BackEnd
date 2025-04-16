namespace JobMagnet.ViewModels.Profile;

public record GalleryViewModel(
    int Position,
    string Title,
    string Description,
    string Link,
    string Image,
    string Type,
    string Video
);