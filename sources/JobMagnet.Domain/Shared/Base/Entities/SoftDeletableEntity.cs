using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Shared.Base.Entities;

public abstract class SoftDeletableEntity<TId> : TrackableEntity<TId>, ISoftDeletable<TId>
{
    protected SoftDeletableEntity()
    {
    }

    protected SoftDeletableEntity(TId id) : base(id)
    {
    }

    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt.HasValue;

    internal void MarkAsDeleted(IClock clock)
    {
        if (IsDeleted) return;
        DeletedAt = clock.UtcNow;
    }

    internal void Restore()
    {
        if (!IsDeleted) return;
        DeletedAt = null;
    }
}