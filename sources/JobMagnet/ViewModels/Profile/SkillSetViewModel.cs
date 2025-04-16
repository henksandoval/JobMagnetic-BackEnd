namespace JobMagnet.ViewModels.Profile;

public record SkillSetViewModel(
    string Overview,
    SkillDetailsViewModel[] SkillDetails
);