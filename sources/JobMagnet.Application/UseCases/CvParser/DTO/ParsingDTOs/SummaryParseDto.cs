namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class SummaryParseDto
{
    public IEnumerable<EducationParseDto> Education { get; set; }
    public IEnumerable<WorkExperienceParseDto> WorkExperiences { get; set; }
    public string? Introduction { get; set; }
}