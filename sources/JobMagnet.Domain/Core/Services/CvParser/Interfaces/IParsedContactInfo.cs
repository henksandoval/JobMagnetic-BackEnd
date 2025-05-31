namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedContactInfo
{
    string? ContactType { get; }
    string? Value { get; }
}