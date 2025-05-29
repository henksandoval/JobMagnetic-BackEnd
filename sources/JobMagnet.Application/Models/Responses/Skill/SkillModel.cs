using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Models.Responses.Skill;

public sealed record SkillModel : IIdentifierBase<long>
{
    public required long Id { get; init; }

    public required SkillBase SkillData { get; init; }
}