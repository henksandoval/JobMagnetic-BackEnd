using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

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
        Guard.IsNotNull(skillType);

        var newSkill = new SkillEntity(proficiencyLevel, 0, this, skillType);
        _skills.Add(newSkill);
    }
}