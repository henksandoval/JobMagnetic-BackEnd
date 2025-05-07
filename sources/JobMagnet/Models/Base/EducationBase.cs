namespace JobMagnet.Models.Base;

public class EducationBase
{
    public long? Id { get; set; }
    public string? Degree { get; set; }
    public string? InstitutionName { get; set; }
    public string? InstitutionLocation { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}