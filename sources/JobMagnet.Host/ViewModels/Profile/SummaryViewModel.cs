namespace JobMagnet.Application.ViewModels.Profile;

public record SummaryViewModel(
    string Introduction,
    EducationViewModel Education,
    WorkExperienceViewModel WorkExperience
);