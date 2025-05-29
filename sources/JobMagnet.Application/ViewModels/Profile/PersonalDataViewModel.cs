namespace JobMagnet.Application.ViewModels.Profile;

public record PersonalDataViewModel(
    string Name,
    string[] Professions,
    SocialNetworksViewModel[] SocialNetworks
);