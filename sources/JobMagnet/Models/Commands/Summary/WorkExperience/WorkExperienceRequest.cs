namespace JobMagnet.Models.Commands.Summary.WorkExperience;

public class WorkExperienceRequest
{
    public long? Id { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string CompanyLocation { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; }
    public ICollection<string> Responsibilities { get; set; } = new List<string>();
}