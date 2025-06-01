namespace JobMagnet.Application.UseCases.CvParser.RawDTOs;

public class SkillRaw
{
    public string? Overview { get; set; }
    public IEnumerable<SkillDetailRaw>? SkillDetails { get; set; }
}