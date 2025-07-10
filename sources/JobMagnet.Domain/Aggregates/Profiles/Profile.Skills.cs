using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile
{
    public void AddSkillSet(SkillSet skillSet)
    {
        Guard.IsNotNull(skillSet);

        SkillSet = skillSet;
    }

    public void UpdateSkillSet(string overview)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.Update(overview);
    }

    public void AddSkill(IGuidGenerator guidGenerator, ushort proficiencyLevel, SkillType skillType)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.AddSkill(guidGenerator, proficiencyLevel, skillType);
    }

    public void UpdateSkill(SkillId skillId, ushort skillProficiencyLevel)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.UpdateSkill(skillId, skillProficiencyLevel);
    }

    public void RemoveSkill(SkillId skillId)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.RemoveSkill(skillId);
    }

    public IReadOnlyCollection<Skill> GetSkills()
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        return SkillSet!.Skills;
    }

    public void ArrangeSkills(IEnumerable<SkillId> orderedSkills)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.ArrangeSkills(orderedSkills);
    }
}