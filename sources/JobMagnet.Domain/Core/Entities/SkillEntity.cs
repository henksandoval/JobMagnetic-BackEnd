using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class SkillEntity : TrackableEntity<long>
{
    public ushort ProficiencyLevel { get; private set; }
    public ushort Rank { get; private set; }
    public SkillType SkillType { get; private set; }

    [ForeignKey(nameof(SkillSet))] public long SkillId { get; private set; }

    public virtual SkillSetEntity SkillSet { get; private set; }

    private SkillEntity() { }

    [SetsRequiredMembers]
    public SkillEntity(SkillSetEntity skillSet, ushort proficiencyLevel, ushort rank, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo<long>(id, 0);
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        Guard.IsGreaterThan<ushort>(rank, 0);
        Guard.IsNotNull(skillSet);

        Id = id;
        ProficiencyLevel = proficiencyLevel;
        Rank = rank;
        SkillId = skillSet.Id;
        SkillSet = skillSet;
    }
}