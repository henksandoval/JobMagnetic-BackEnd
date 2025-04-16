namespace JobMagnet.ViewModels.Profile;

public record AboutViewModel(
    string ImageUrl,
    string Description,
    string Text,
    string Hobbies,
    DateOnly Birthday,
    string Website,
    string PhoneNumber,
    string City,
    ushort Age,
    string Degree,
    string Email,
    string Freelance,
    string WorkExperience
);