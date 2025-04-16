namespace JobMagnet.ViewModels.Profile;

public record SummaryViewModel(
    string About,
    string Name,
    string Introduction,
    ContactProfileViewModel ContactProfileViewModel,
    EducationViewModel EducationViewModel,
    WorkExperienceViewModel WorkExperienceViewModel
);