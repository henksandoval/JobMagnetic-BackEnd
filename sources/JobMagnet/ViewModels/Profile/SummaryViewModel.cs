namespace JobMagnet.ViewModels.Profile;

public record SummaryViewModel(
    string Introduction,
    EducationViewModel EducationViewModel,
    WorkExperienceViewModel WorkExperienceViewModel
);