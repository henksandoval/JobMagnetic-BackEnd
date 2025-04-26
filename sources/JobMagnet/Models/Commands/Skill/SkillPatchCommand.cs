using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Skill;

public sealed class SkillPatchCommand : SkillBase
{
    public long? Id { get; init; }
}