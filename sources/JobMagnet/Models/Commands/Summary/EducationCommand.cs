namespace JobMagnet.Models.Commands.Summary.Education;

public class EducationCommand
{
    public long? Id { get; set; }
    public string Degree { get; set; }
    public string InstitutionName { get; set; }
    public string InstitutionLocation { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; }
}