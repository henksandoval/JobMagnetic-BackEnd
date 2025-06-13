using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class SkillSetEntity : SoftDeletableEntity<long>
{
    public string? Overview { get; set; }
    public virtual ICollection<SkillEntity> Skills { get; set; } = new HashSet<SkillEntity>();

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }

    public void Add(SkillEntity skill)
    {
        Guard.IsNotNull(skill);

        Skills.Add(skill);
    }
}