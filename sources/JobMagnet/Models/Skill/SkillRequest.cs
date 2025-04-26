using JobMagnet.Models.Base;

namespace JobMagnet.Models.Skill;

public sealed class SkillRequest : SkillBase
{
    public long? Id { get; init; }
}