using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Skill;

public sealed record SkillModel : IIdentifierBase<long>
{
    public required long Id { get; init; }

    public required SkillBase SkillData { get; init; }
}