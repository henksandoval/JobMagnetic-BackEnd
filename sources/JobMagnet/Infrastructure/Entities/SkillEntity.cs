using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

public class SkillEntity : SoftDeletableEntity<long>
{
    public string? Overview { get; set; }
    public virtual ICollection<SkillItemEntity> SkillDetails { get; set; } = new HashSet<SkillItemEntity>();

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
}