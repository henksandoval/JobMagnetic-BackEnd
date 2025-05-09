using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Skill;

public sealed record SkillCommand
{
    public required SkillBase SkillData { get; init; }
}