namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class WorkExperienceDTO
{
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string CompanyLocation { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; }
    public ICollection<string> Responsibilities { get; set; } = new List<string>();

    public virtual SummaryDTO Summary { get; set; }
}