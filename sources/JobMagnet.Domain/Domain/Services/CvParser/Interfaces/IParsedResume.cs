namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedResume
{
    string? JobTitle { get; }
    string? About { get; }
    string? Summary { get; }
    string? Overview { get; }
    string? Title { get; }
    string? Suffix { get; }
    string? Address { get; }
    IReadOnlyCollection<IParsedContactInfo> ContactInfo { get; }
}