using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Skill;

public sealed record SkillCommand
{
    public required SkillBase SkillData { get; init; }
}