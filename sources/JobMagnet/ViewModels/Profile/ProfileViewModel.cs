namespace JobMagnet.ViewModels.Profile;

public record ProfileViewModel(
    PersonalDataViewModel? PersonalData = null,
    AboutViewModel? About =  null,
    TestimonialsViewModel[]? Testimonials = null,
    SkillSetViewModel? SkillSet = null,
    TitlesViewModel? Titles = null,
    SummaryViewModel? Summary = null,
    PortfolioViewModel? Portfolio = null,
    ServiceViewModel? Service = null,
    ContactViewModel? Contact = null
    );