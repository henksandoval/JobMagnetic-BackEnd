using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Skill;

public sealed class SkillModel : SkillBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}