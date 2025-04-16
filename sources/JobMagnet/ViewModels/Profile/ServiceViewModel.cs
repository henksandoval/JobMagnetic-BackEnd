namespace JobMagnet.ViewModels.Profile;

public record ServiceViewModel(
    string Title,
    string Overview,
    ServiceDetailsViewModel[] ServiceDetails
);