using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class SkillSet : SoftDeletableEntity<SkillSetId>
{
    private readonly HashSet<Skill> _skills = [];

    public string? Overview { get; private set; }
    public virtual IReadOnlyCollection<Skill> Skills => _skills;

    public ProfileId ProfileId { get; private set; }

    private SkillSet()
    {
    }

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

    public void ArrangeSkills(IEnumerable<SkillId> orderedSkills)
    {
        var skillIds = orderedSkills.ToList();
        Guard.IsNotNull(skillIds);

        var currentSkillIds = new HashSet<SkillId>(_skills.Select(p => p.Id));

        if (skillIds.Count != new HashSet<SkillId>(skillIds).Count)
            throw new BusinessRuleValidationException("The list of skill IDs for reordering contains duplicates.");

        if (!currentSkillIds.SetEquals(skillIds))
            throw new BusinessRuleValidationException(
                "The provided Skill list for reordering does not match the skills in the SkillSet. Ensure all skills are included exactly once.");

        foreach (var (skillId, index) in skillIds.WithIndex())
        {
            var position = (ushort)(index + 1);
            var skillToUpdate = _skills.Single(p => p.Id == skillId);
            skillToUpdate.UpdatePosition(position);
        }
    }

    internal void AddSkill(IGuidGenerator guidGenerator, ushort proficiencyLevel, SkillType skillType)
    {
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        Guard.IsNotNull(skillType);

        if (_skills.Any(s => s.SkillTypeId == skillType.Id))
            throw new JobMagnetDomainException($"Skill of type {skillType.Name} already exists in this skill set.");

        var position = GetPosition();

        var newSkill = Skill.CreateInstance(guidGenerator, Id, skillType, proficiencyLevel, position);
        _skills.Add(newSkill);
    }

    internal bool SkillExists(SkillType skillType)
    {
        Guard.IsNotNull(skillType);

        return _skills.Any(s => s.SkillTypeId == skillType.Id || s.SkillType.Name == skillType.Name);
    }

    internal void RemoveSkill(SkillId skillId)
    {
        var skillToRemove = Skills.FirstOrDefault(p => p.Id == skillId);

        if (skillToRemove is null)
            throw NotFoundException.For<Skill, SkillId>(skillId);

        _skills.Remove(skillToRemove);
    }

    internal void Update(string overview)
    {
        Guard.IsNotNullOrEmpty(overview);
        var isSameOverview = string.Equals(Overview, overview, StringComparison.OrdinalIgnoreCase);
        if (isSameOverview) return;

        Overview = overview;
    }

    internal void UpdateSkill(SkillId skillId, ushort newProficiencyLevel)
    {
        var skillToUpdate = _skills.FirstOrDefault(s => s.Id == skillId);

        if (skillToUpdate is null) throw new JobMagnetDomainException($"Skill with ID '{skillId.Value}' not found in this profile.");

        skillToUpdate.UpdateProficiencyLevel(newProficiencyLevel);
    }

    private ushort GetPosition()
    {
        return (ushort)(_skills.Count > 0 ? _skills.Max(x => x.Position) + 1 : 1);
    }
}