using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Shared.Base.Aggregates;

public abstract class SoftDeletableAggregateRoot<TId> : TrackableAggregateRoot<TId>
{
    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt.HasValue;

    protected SoftDeletableAggregateRoot() : base() { }

    protected SoftDeletableAggregateRoot(TId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt)
    {
        DeletedAt = deletedAt?.UtcDateTime;
    }

    protected SoftDeletableAggregateRoot(TId id, IClock clock) : base(id, clock)
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

    protected void Restore(IClock clock)
    {
        if (!IsDeleted) return;

        DeletedAt = null;
        AddDomainEvent(new EntityRestoredEvent(Id, GetType().Name, clock.UtcNow));
    }
}

public record EntityRestoredEvent(object Id, string Name, DateTimeOffset ClockUtcNow) : IDomainEvent;

public record EntityDeletedEvent(object Id, string Name, DateTimeOffset ClockUtcNow) : IDomainEvent;