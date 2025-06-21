using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities.Skills;

public class SkillSetEntity : SoftDeletableEntity<long>
{
    private readonly HashSet<SkillEntity> _skills = [];

    public string? Overview { get; private set; }
    public virtual IReadOnlyCollection<SkillEntity> Skills => _skills;

    [ForeignKey(nameof(Profile))] public long ProfileId { get; private set; }

    public virtual ProfileEntity Profile { get; private set; }

    private SkillSetEntity() { }

    [SetsRequiredMembers]
    public SkillSetEntity(string overview, long profileId, long id = 0)
    {
        Guard.IsNotNullOrWhiteSpace(overview);
        Guard.IsGreaterThanOrEqualTo(profileId, 0);
        Guard.IsGreaterThanOrEqualTo<long>(id, 0);

        Id = id;
        Overview = overview;
        ProfileId = profileId;
    }

    public void AddSkill(ushort proficiencyLevel, SkillType skillType)
    {
        Guard.IsBetweenOrEqualTo<ushort>(proficiencyLevel, 0, 10);
        Guard.IsNotNull(skillType);

        if (_skills.Any(s => s.SkillTypeId > 0 && s.SkillTypeId == skillType.Id))
        {
            throw new JobMagnetDomainException($"Skill of type {skillType.Name} already exists in this skill set.");
        }

        var newRank = (ushort)(_skills.Count + 1);

        var newSkill = new SkillEntity(proficiencyLevel, newRank, this, skillType);
        _skills.Add(newSkill);
    }

    public bool SkillExists(SkillType skillType)
    {
        Guard.IsNotNull(skillType);

        return _skills.Any(s => s.SkillTypeId == skillType.Id);
    }

    public void ReorderSkills(List<long> orderedSkillIds)
    {
        Guard.IsNotNull(orderedSkillIds);

        var currentSkillIds = _skills.Select(s => s.Id).ToHashSet();
        if (!currentSkillIds.SetEquals(orderedSkillIds))
        {
            throw new JobMagnetDomainException("The provided skill IDs do not match the current skills in this skill set.");
        }

        var skillMap = _skills.ToDictionary(s => s.Id);

        ushort currentRank = 1;
        foreach (var skillToUpdate in orderedSkillIds.Select(skillId => skillMap[skillId]))
        {
            skillToUpdate.UpdateRank(currentRank);
            currentRank++;
        }
    }
}