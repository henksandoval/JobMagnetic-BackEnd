namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record SkillRaw(string? Overview, ICollection<SkillDetailRaw>? SkillDetails);