namespace JobMagnet.Application.ViewModels.Profile;

public record ServiceViewModel(
    string Overview,
    ServiceDetailsViewModel[] ServiceDetails
);