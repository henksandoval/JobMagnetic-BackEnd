using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Skills;

public readonly record struct SkillId(Guid Value) : IStronglyTypedId<SkillId>;

public class Skill : TrackableAggregate<SkillId>
{
    public ushort ProficiencyLevel { get; private set; }
    public ushort Rank { get; private set; }
    public SkillSetId SkillSetId { get; private set; }
    public SkillTypeId SkillTypeId { get; private set; }
    public virtual SkillType SkillType { get; private set; }

    private Skill(SkillId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt) :
        base(id, addedAt, lastModifiedAt)
    {
    }

    private Skill(SkillId id, IClock clock, SkillSetId skillSetId, SkillType skillType, ushort proficiencyLevel, ushort rank) : base(id, clock)
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

    internal static Skill CreateInstance(IGuidGenerator guidGenerator, IClock clock, SkillSetId skillSetId, SkillType skillType,
        ushort proficiencyLevel, ushort rank)
    {
        var id = new SkillId(guidGenerator.NewGuid());
        return new Skill(id, clock, skillSetId, skillType, proficiencyLevel, rank);
    }

    internal void UpdateRank(ushort newRank)
    {
        Guard.IsGreaterThan<ushort>(newRank, 0);

        Rank = newRank;
    }
}