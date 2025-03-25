using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class SkillEntity : SoftDeletableEntity<long>
{
    public string? Overview { get; set; }
    public virtual ICollection<SkillItemEntity> SkillDetails { get; set; } = new HashSet<SkillItemEntity>();
}