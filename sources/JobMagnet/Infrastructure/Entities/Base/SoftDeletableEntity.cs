using JobMagnet.Infrastructure.Entities.Base.Interfaces;

namespace JobMagnet.Infrastructure.Entities.Base;

public abstract class SoftDeletableEntity<TId> : TrackableEntity<TId>, ISoftDeletableEntity<TId>
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}