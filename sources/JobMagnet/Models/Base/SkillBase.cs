using JobMagnet.Models.Commands.Skill;

namespace JobMagnet.Models.Base;

public abstract class SkillBase
{
    public required long ProfileId { get; set; }
    public required IList<SkillItemCommand> SkillDetails { get; set; }
}