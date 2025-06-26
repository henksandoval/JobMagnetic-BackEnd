using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Shared.Base;

public abstract class SoftDeletableEntity<TId> : TrackableEntity<TId>, ISoftDeletableEntity<TId>
{
    protected SoftDeletableEntity(TId id, Guid addedBy) : base(id, addedBy)
    {
    }

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }

    public virtual void Delete(Guid? deletedBy = null)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;

        UpdateModificationDetails(deletedBy);

        //TODO: AddDomainEvent(new EntitySoftDeletedEvent(this));
    }

    public virtual void Restore()
    {
        if (!IsDeleted) return;

        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;

        //TODO: AddDomainEvent(new EntityRestoredEvent(this));
    }
}