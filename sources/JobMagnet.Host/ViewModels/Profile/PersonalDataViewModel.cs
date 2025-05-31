namespace JobMagnet.Host.ViewModels.Profile;

public record PersonalDataViewModel(
    string Name,
    string[] Professions,
    SocialNetworksViewModel[] SocialNetworks
);