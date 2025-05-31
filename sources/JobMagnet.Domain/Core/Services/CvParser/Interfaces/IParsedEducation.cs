namespace JobMagnet.Domain.Core.Services.CvParser.Interfaces;

public interface IParsedEducation
{
    string? Degree { get; }
    string? InstitutionName { get; }
    string? InstitutionLocation { get; }
    string? Description { get; }
    DateOnly? StartDate { get; }
    DateOnly? EndDate { get; }
}