namespace JobMagnet.Host.ViewModels.Profile;

public record SkillDetailsViewModel(
    string Name,
    string IconUrl,
    ushort Rank
);