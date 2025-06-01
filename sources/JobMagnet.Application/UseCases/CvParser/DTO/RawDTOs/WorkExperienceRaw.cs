namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public class WorkExperienceRaw
{
    public string? JobTitle { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyLocation { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Description { get; set; }
}