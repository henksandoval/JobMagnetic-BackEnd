namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedEducation
{
    string? SchoolName { get; }
    string? Degree { get; }
    string? FieldOfStudy { get; }
    DateOnly? StartDate { get; }
    DateOnly? EndDate { get; }
}