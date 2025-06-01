namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record SummaryRaw(
    string? Introduction,
    ICollection<EducationRaw>? Education,
    ICollection<WorkExperienceRaw>? WorkExperiences);