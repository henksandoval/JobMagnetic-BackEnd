namespace JobMagnet.Host.ViewModels.Profile;

public record SkillSetViewModel(
    string Overview,
    SkillDetailsViewModel[] SkillDetails
);