using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct SkillSetId(Guid Value) : IStronglyTypedId<SkillSetId>;

public class SkillSet : SoftDeletableEntity<SkillSetId>
{
    private readonly HashSet<Skill> _skills = [];

    public string? Overview { get; private set; }
    public virtual IReadOnlyCollection<Skill> Skills => _skills;

    public ProfileId ProfileId { get; private set; }

    private SkillSet() : base() { }

    private SkillSet(SkillSetId id, ProfileId profileId, string overview) : base(id)
    {
        Guard.IsNotNullOrWhiteSpace(overview);
        Guard.IsNotNull(profileId);

        Id = id;
        Overview = overview;
        ProfileId = profileId;
    }

    public static SkillSet CreateInstance(IGuidGenerator guidGenerator, ProfileId profileId, string overview)
    {
        var id = new SkillSetId(guidGenerator.NewGuid());
        return new SkillSet(id, profileId, overview);
    }

    public void AddSkill(IGuidGenerator guidGenerator, IClock clock, ushort proficiencyLevel, SkillType skillType)
    {
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        Guard.IsNotNull(skillType);

        if (_skills.Any(s => s.SkillTypeId == skillType.Id))
            throw new JobMagnetDomainException($"Skill of type {skillType.Name} already exists in this skill set.");

        var newRank = (ushort)(_skills.Count + 1);

        var newSkill = Skill.CreateInstance(guidGenerator, Id, skillType, proficiencyLevel, newRank);
        _skills.Add(newSkill);
    }

    public bool SkillExists(SkillType skillType)
    {
        Guard.IsNotNull(skillType);

        return _skills.Any(s => s.SkillTypeId == skillType.Id || s.SkillType.Name == skillType.Name);
    }

    public void RemoveSkill(SkillId skillId)
    {
        var skillToRemove = Skills.FirstOrDefault(p => p.Id == skillId);

        if (skillToRemove is null)
            throw NotFoundException.For<Skill, SkillId>(skillId);

        _skills.Remove(skillToRemove);
    }
}