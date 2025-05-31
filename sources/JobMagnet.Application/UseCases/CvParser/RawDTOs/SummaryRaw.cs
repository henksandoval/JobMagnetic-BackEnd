namespace JobMagnet.Application.UseCases.CvParser.RawDTOs;

public class SummaryRaw
{
    public string? Introduction { get; set; }
    public IEnumerable<EducationRaw> Education { get; set; }
    public IEnumerable<WorkExperienceRaw> WorkExperiences { get; set; }
}