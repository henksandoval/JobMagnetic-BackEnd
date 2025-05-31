using JobMagnet.Domain.Core.Entities.Base.Interfaces;

namespace JobMagnet.Domain.Core.Entities.Base;

public abstract class SoftDeletableEntity<TId> : TrackableEntity<TId>, ISoftDeletableEntity<TId>
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}