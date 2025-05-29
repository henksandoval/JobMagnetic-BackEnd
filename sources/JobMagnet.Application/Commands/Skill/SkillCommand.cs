using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Commands.Skill;

public sealed record SkillCommand
{
    public required SkillBase SkillData { get; init; }
}