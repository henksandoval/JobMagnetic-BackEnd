using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Skill;

public sealed record SkillUpdateCommand
{
    public required long Id { get; init; }
    public required SkillBase SkillData { get; init; }
}