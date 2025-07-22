using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Skill;

public sealed record SkillResponse : IIdentifierBase<Guid>
{
    public required SkillSetBase SkillSetData { get; init; }
    public required Guid Id { get; init; }
}