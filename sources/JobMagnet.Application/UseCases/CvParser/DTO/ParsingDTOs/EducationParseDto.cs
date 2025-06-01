namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class EducationParseDto
{
    public string? Degree { get; set; }
    public string? InstitutionName { get; set; }
    public string? InstitutionLocation { get; set; }
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}