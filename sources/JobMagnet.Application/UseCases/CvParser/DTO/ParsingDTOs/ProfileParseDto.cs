namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class ProfileParseDto
{
    public ResumeParseDto? Resume { get; set; }
    public SkillParseDto? Skill { get; set; }
    public ServiceParseDto? Services { get; set; }
    public SummaryParseDto? Summary { get; set; }
    public List<TalentParseDto> Talents { get; set; }
    public List<PortfolioGalleryParseDto> PortfolioGallery { get; set; }
    public List<TestimonialParseDto> Testimonials { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }
}