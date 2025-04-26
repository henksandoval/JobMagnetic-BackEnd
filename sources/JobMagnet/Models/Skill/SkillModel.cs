using JobMagnet.Models.Base;

namespace JobMagnet.Models.Skill;

public sealed class SkillModel : SkillBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}