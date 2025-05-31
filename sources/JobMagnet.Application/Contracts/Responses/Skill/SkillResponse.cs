using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Skill;

public sealed record SkillResponse : IIdentifierBase<long>
{
    public required SkillBase SkillData { get; init; }
    public required long Id { get; init; }
}