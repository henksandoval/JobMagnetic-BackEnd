namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record WorkExperienceBase(
    string? JobTitle,
    string? CompanyName,
    string? CompanyLocation,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    ICollection<string>? Responsibilities,
    Guid Id);