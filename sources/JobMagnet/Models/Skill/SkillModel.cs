using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Skill;

public sealed class SkillModel : SkillBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}