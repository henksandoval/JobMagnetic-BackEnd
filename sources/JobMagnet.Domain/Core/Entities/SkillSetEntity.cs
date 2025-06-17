using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class SkillSetEntity : SoftDeletableEntity<long>
{
    public string? Overview { get; private set; }
    public virtual ICollection<SkillEntity> Skills { get; private set; } = new HashSet<SkillEntity>();

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

    public void Add(SkillEntity skill)
    {
        Guard.IsNotNull(skill);

        Skills.Add(skill);
    }

    public void AddRange(List<SkillEntity> skills)
    {
        foreach (var skill in skills)
            Add(skill);
    }
}