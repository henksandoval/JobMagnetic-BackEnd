namespace JobMagnet.Models.Base;

public class WorkExperienceBase
{
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string CompanyLocation { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; }
    public ICollection<string> Responsibilities { get; set; } = new List<string>();
}