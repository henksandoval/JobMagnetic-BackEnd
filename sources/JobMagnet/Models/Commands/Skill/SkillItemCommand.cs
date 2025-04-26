using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Skill;

public sealed class SkillItemCommand : SkillItemBase
{
    public long? Id { get; set; }
}