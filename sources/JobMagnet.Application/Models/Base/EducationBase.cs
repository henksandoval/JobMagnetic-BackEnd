namespace JobMagnet.Application.Models.Base;

public sealed record EducationBase
{
    public long? Id { get; init; }
    public string? Degree { get; init; }
    public string? InstitutionName { get; init; }
    public string? InstitutionLocation { get; init; }
    public string? Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}