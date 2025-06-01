namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record ProfileRaw
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }

    public ResumeRaw? Resume { get; set; }
    public SkillRaw? Skill { get; set; }
    public ServiceRaw? Services { get; set; }
    public SummaryRaw? Summary { get; set; }

    public List<TalentRaw> Talents { get; set; }
    public List<PortfolioGalleryRaw> PortfolioGallery { get; set; }
    public List<TestimonialRaw>? Testimonials { get; set; }
}