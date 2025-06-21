using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.Skills;

public class Skill : TrackableEntity<long>
{
    public ushort ProficiencyLevel { get; private set; }
    public ushort Rank { get; private set; }
    public long SkillSetId { get; private set; }
    public int SkillTypeId { get; private set; }
    public virtual SkillType SkillType { get; private set; }
    public virtual SkillSet SkillSet { get; private set; }

    private Skill() { }

    [SetsRequiredMembers]
    internal Skill(ushort proficiencyLevel, ushort rank, SkillSet skillSet, SkillType skillType, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo<long>(id, 0);
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        Guard.IsGreaterThan<ushort>(rank, 0);
        Guard.IsNotNull(skillSet);
        Guard.IsNotNull(skillType);

        Id = id;
        ProficiencyLevel = proficiencyLevel;
        Rank = rank;
        SkillSetId = skillSet.Id;
        SkillSet = skillSet;
        SkillTypeId = skillType.Id;
        SkillType = skillType;
    }

    internal void UpdateRank(ushort newRank)
    {
        Guard.IsGreaterThan<ushort>(newRank, 0);

        Rank = newRank;
    }
}