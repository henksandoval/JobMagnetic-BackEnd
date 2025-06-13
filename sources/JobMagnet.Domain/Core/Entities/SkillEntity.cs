using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class SkillEntity : SoftDeletableEntity<long>
{
    public string? Overview { get; set; }
    public virtual ICollection<SkillItemEntity> SkillDetails { get; set; } = new HashSet<SkillItemEntity>();

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }

    public void Add(SkillItemEntity skillItem)
    {
        Guard.IsNull(skillItem);

        SkillDetails.Add(skillItem);
    }
}