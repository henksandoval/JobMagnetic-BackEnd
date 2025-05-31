namespace JobMagnet.Domain.Core.Services.CvParser.Interfaces;

public interface IParsedContactInfo
{
    string? ContactType { get; }
    string? Value { get; }
}