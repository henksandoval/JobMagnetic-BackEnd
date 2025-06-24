namespace JobMagnet.Host.ViewModels.Profile;

public record ProjectViewModel(
    int Position,
    string Title,
    string Description,
    string Link,
    string Image,
    string Type,
    string Video
);