namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record ResumeRaw(
    string? JobTitle,
    string? About,
    string? Summary,
    string? Overview,
    string? Title,
    string? Suffix,
    string? Address,
    ICollection<ContactInfoRaw> ContactInfo);