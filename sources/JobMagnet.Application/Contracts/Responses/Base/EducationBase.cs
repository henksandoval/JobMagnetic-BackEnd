namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record EducationBase(
    string? Degree,
    string? InstitutionName,
    string? InstitutionLocation,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    long Id = 0);