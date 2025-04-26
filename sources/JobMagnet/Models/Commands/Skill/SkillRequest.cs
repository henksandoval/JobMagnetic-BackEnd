using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Skill;

public sealed class SkillRequest : SkillBase
{
    public long? Id { get; init; }
}