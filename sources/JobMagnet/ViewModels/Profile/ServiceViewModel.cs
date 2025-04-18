namespace JobMagnet.ViewModels.Profile;

public record ServiceViewModel(
    string Overview,
    ServiceDetailsViewModel[] ServiceDetails
);