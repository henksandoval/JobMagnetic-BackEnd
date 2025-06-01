namespace JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

public class SkillRaw
{
    public string? Overview { get; set; }
    public IEnumerable<SkillDetailRaw>? SkillDetails { get; set; }
}