namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class WorkExperienceParseDto
{
    public string? JobTitle { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyLocation { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
}