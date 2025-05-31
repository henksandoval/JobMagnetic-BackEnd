using JobMagnet.Domain.Core.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class EducationParseDto : IParsedEducation
{
    public string? Degree { get; set; }
    public string? InstitutionName { get; set; }
    public string? InstitutionLocation { get; set; }
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}