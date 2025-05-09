namespace JobMagnet.Models.Base;

public sealed record WorkExperienceBase
{
    public long Id { get; init; }
    public string? JobTitle { get; init; }
    public string? CompanyName { get; init; }
    public string? CompanyLocation { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public ICollection<string> Responsibilities { get; init; } = new List<string>();
}