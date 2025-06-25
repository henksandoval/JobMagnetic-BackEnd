using JobMagnet.Domain.Core.Entities.Base.Interfaces;

namespace JobMagnet.Domain.Core.Entities.Base;

public abstract class SoftDeletableEntity<TId> : TrackableEntity<TId>, ISoftDeletableEntity<TId>
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public virtual void Delete(Guid? deletedBy = null)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;

        //TODO: AddDomainEvent(new EntitySoftDeletedEvent(this));
    }

    public virtual void Restore()
    {
        if (!IsDeleted) return;

        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;

        //TODO: AddDomainEvent(new EntitySoftDeletedEvent(this));
    }
}