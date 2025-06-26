using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Skill;

public sealed record SkillResponse : IIdentifierBase<Guid>
{
    public required SkillBase SkillData { get; init; }
    public required Guid Id { get; init; }
}