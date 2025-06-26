using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct SkillSetId(Guid Value) : IStronglyTypedId<Guid>;

public class SkillSet : SoftDeletableEntity<SkillSetId>
{
    private readonly HashSet<Skill> _skills = [];

    public string? Overview { get; private set; }
    public virtual IReadOnlyCollection<Skill> Skills => _skills;

    public ProfileId ProfileId { get; private set; }

    private SkillSet() : base(new SkillSetId(), Guid.Empty)
    {
    }

    public SkillSet(string overview, ProfileId profileId, SkillSetId id) : base(id, Guid.Empty)
    {
        Guard.IsNotNullOrWhiteSpace(overview);

        Id = id;
        Overview = overview;
        ProfileId = profileId;
    }

    public void AddSkill(ushort proficiencyLevel, SkillType skillType)
    {
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        Guard.IsNotNull(skillType);

        if (_skills.Any(s => s.SkillTypeId == skillType.Id))
            throw new JobMagnetDomainException($"Skill of type {skillType.Name} already exists in this skill set.");

        var newRank = (ushort)(_skills.Count + 1);

        var newSkill = new Skill(new SkillId(), Id, skillType, proficiencyLevel, newRank);
        _skills.Add(newSkill);
    }

    public bool SkillExists(SkillType skillType)
    {
        Guard.IsNotNull(skillType);

        return _skills.Any(s => s.SkillTypeId == skillType.Id);
    }
}