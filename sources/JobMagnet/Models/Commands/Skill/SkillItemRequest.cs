using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Skill;

public sealed class SkillItemRequest : SkillItemBase
{
    public long? Id { get; set; }
}