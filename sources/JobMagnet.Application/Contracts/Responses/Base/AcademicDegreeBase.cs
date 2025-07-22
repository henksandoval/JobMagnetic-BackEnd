namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record AcademicDegreeBase(
    string? Degree,
    string? InstitutionName,
    string? InstitutionLocation,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    Guid Id);