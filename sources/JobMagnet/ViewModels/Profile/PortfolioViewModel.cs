namespace JobMagnet.ViewModels.Profile;

public record PortfolioViewModel(
    int Position,
    string Title,
    string Description,
    string Link,
    string Image,
    string Type,
    string Video
);