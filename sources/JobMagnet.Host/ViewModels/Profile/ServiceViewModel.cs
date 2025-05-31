namespace JobMagnet.Host.ViewModels.Profile;

public record ServiceViewModel(
    string Overview,
    ServiceDetailsViewModel[] ServiceDetails
);