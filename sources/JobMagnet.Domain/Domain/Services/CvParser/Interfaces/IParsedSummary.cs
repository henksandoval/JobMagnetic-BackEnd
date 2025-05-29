namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedSummary
{
    string? Introduction { get; }
    IReadOnlyCollection<IParsedEducation> Education { get; }
    IReadOnlyCollection<IParsedWorkExperience> WorkExperiences { get; }
}