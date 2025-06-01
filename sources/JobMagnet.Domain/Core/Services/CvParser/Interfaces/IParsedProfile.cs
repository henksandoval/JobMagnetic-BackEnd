namespace JobMagnet.Domain.Core.Services.CvParser.Interfaces;

public interface IParsedProfile
{
    string? FirstName { get; }
    string? LastName { get; }
    string? ProfileImageUrl { get; }
    DateOnly? BirthDate { get; }
    string? MiddleName { get; }
    string? SecondLastName { get; }

    // IParsedResume? Resume { get; }
    IParsedSkill? Skill { get; }
    IParsedService? Services { get; }
    IParsedSummary? Summary { get; }
    IReadOnlyCollection<IParsedTalent> Talents { get; }
    IReadOnlyCollection<IParsedPortfolioGallery> PortfolioGallery { get; }
    IReadOnlyCollection<IParsedTestimonial> Testimonials { get; }
}