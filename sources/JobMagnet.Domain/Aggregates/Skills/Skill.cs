using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Skills;

public readonly record struct SkillId(Guid Value) : IStronglyTypedId<Guid>;

public class Skill : TrackableEntity<SkillId>
{
    public ushort ProficiencyLevel { get; private set; }
    public ushort Rank { get; private set; }
    public SkillSetId SkillSetId { get; private set; }
    public SkillTypeId SkillTypeId { get; private set; }
    public virtual SkillType SkillType { get; private set; }

    private Skill() : base(new SkillId(), Guid.Empty)
    {
    }

    internal Skill(SkillId id, SkillSetId skillSetId, SkillType skillType, ushort proficiencyLevel, ushort rank) : base(id, Guid.Empty)
    {
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        Guard.IsGreaterThan<ushort>(rank, 0);
        Guard.IsNotNull(skillType);

        Id = id;
        SkillSetId = skillSetId;
        ProficiencyLevel = proficiencyLevel;
        Rank = rank;
        SkillTypeId = skillType.Id;
        SkillType = skillType;
    }

    internal void UpdateRank(ushort newRank)
    {
        Guard.IsGreaterThan<ushort>(newRank, 0);

        Rank = newRank;
    }
}