using JobMagnet.Domain.Entities.Base.Interfaces;

namespace JobMagnet.Domain.Entities.Base;

public abstract class SoftDeletableEntity<TId> : TrackableEntity<TId>, ISoftDeletableEntity<TId>
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}