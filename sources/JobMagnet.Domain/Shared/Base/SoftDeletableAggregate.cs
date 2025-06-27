using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Shared.Base;

public abstract class SoftDeletableAggregate<TId> : TrackableAggregate<TId>
{
    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt.HasValue;

    protected SoftDeletableAggregate(TId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt)
    {
        DeletedAt = deletedAt?.UtcDateTime;
    }

    protected SoftDeletableAggregate(TId id, IClock clock) : base(id, clock)
    {
        DeletedAt = null;
    }

    protected void Remove(IClock clock)
    {
        if (IsDeleted) return;

        var deleteTimeOffSet = clock.UtcNow;
        DeletedAt = deleteTimeOffSet;
        AddDomainEvent(new EntityDeletedEvent(Id, GetType().Name, deleteTimeOffSet));
    }

    public void Restore(IClock clock)
    {
        if (!IsDeleted) return;

        DeletedAt = null;
        AddDomainEvent(new EntityRestoredEvent(Id, GetType().Name, clock.UtcNow));
    }
}

public record EntityRestoredEvent(object Id, string Name, DateTimeOffset ClockUtcNow) : IDomainEvent;

public record EntityDeletedEvent(object Id, string Name, DateTimeOffset ClockUtcNow) : IDomainEvent;