using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class SkillEntity : TrackableEntity<long>
{
    public ushort ProficiencyLevel { get; private set; }
    public ushort Rank { get; private set; }

    [ForeignKey(nameof(SkillSet))] public long SkillId { get; private set; }
    [ForeignKey(nameof(SkillType))] public int SkillTypeId { get; private set; }

    public virtual SkillType SkillType { get; private set; }
    public virtual SkillSetEntity SkillSet { get; private set; }

    private SkillEntity() { }

    [SetsRequiredMembers]
    public SkillEntity(ushort proficiencyLevel, ushort rank, SkillSetEntity skillSet, SkillType skillType, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo<long>(id, 0);
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        //Guard.IsGreaterThan<ushort>(rank, 0); //TODO: Enable again
        Guard.IsNotNull(skillSet);
        Guard.IsNotNull(skillType);

        Id = id;
        ProficiencyLevel = proficiencyLevel;
        Rank = rank;
        SkillId = skillSet.Id;
        SkillTypeId = skillType.Id;
        SkillSet = skillSet;
        SkillType = skillType;
    }

    [SetsRequiredMembers]
    public SkillEntity(ushort proficiencyLevel, ushort rank, SkillType skillType, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo<long>(id, 0);
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        //Guard.IsGreaterThan<ushort>(rank, 0); //TODO: Enable again
        Guard.IsNotNull(skillType);

        Id = id;
        ProficiencyLevel = proficiencyLevel;
        Rank = rank;
        SkillType = skillType;
        SkillTypeId = skillType.Id;
    }
}