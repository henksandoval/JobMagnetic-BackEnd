namespace JobMagnet.Models.Skill;

public abstract class SkillBase
{
    public required IList<SkillItemRequest> SkillDetails { get; set; }
}