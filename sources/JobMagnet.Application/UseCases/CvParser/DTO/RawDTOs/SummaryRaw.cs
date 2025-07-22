namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record SummaryRaw(
    string? Introduction,
    ICollection<AcademicDegreeRaw>? Education,
    ICollection<WorkExperienceRaw>? WorkExperiences);