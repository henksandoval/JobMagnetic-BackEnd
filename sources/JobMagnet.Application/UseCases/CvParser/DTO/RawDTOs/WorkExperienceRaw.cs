namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record WorkExperienceRaw(
    string? JobTitle,
    string? CompanyName,
    string? CompanyLocation,
    string? StartDate,
    string? EndDate,
    string? Description,
    ICollection<ResponsibilityRaw>? Responsibilities);