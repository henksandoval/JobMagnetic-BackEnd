namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record AcademicDegreeRaw(
    string? Degree,
    string? InstitutionName,
    string? InstitutionLocation,
    string? Description,
    string? StartDate,
    string? EndDate);