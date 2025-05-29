namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedContactInfo
{
    string? Type { get; }
    string? Value { get; }
}