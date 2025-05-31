namespace JobMagnet.Domain.Core.Services.CvParser.Interfaces;

public interface IParsedWorkExperience
{
    string JobTitle { get; }
    string? CompanyName { get; }
    string? CompanyLocation { get; }
    DateOnly? StartDate { get; }
    DateOnly? EndDate { get; }
    string? Description { get; }
}