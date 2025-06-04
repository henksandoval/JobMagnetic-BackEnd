namespace JobMagnet.Host.ViewModels.Profile;

public record PositionViewModel(
    string Specialist,
    string StartDate,
    string Location,
    string Description,
    string[] Responsibilities
);