using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Skill;

public sealed record SkillModel : IIdentifierBase<long>
{
    public required long Id { get; init; }

    public required SkillBase SkillData { get; init; }
}