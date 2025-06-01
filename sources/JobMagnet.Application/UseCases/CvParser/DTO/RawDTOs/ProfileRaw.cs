namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record ProfileRaw(
    string? FirstName,
    string? LastName,
    string? ProfileImageUrl,
    string? BirthDate,
    string? MiddleName,
    string? SecondLastName,
    ResumeRaw? Resume,
    SkillRaw? Skill,
    ServiceRaw? Services,
    SummaryRaw? Summary,
    List<TalentRaw> Talents,
    List<PortfolioGalleryRaw> PortfolioGallery,
    List<TestimonialRaw>? Testimonials);