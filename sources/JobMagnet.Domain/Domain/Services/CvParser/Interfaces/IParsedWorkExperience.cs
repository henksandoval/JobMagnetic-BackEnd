namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedWorkExperience
{
    string? CompanyName { get; }
    string? Position { get; }
    DateOnly? StartDate { get; }
    DateOnly? EndDate { get; }
    string? Description { get; }
}