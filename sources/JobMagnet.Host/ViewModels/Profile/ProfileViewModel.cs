namespace JobMagnet.Host.ViewModels.Profile;

public record ProfileViewModel(
    PersonalDataViewModel? PersonalData = null,
    AboutViewModel? About = null,
    TestimonialsViewModel[]? Testimonials = null,
    SkillSetViewModel? SkillSet = null,
    TitlesViewModel? Titles = null,
    SummaryViewModel? Summary = null,
    PortfolioViewModel[]? PortfolioGallery = null,
    ContactViewModel? Contact = null
);