namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public sealed record SkillSetRaw(string? Overview, ICollection<SkillRaw>? Skills);