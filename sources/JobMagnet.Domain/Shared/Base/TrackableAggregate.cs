using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Shared.Base;

public abstract class TrackableAggregate<TId> : BaseAggregate<TId>
{
    public DateTimeOffset AddedAt { get; private set; }
    public DateTimeOffset? LastModifiedAt { get; private set; }

    protected TrackableAggregate() : base() {}

    protected TrackableAggregate(TId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt) : base(id)
    {
        AddedAt = addedAt;
        LastModifiedAt = lastModifiedAt;
    }

    protected TrackableAggregate(TId id, IClock clock) : base(id)
    {
        AddedAt = clock.UtcNow;
        AddDomainEvent(new EntityCreatedEvent(Id, GetType().Name, clock.UtcNow));
    }

    protected virtual void UpdateModificationDetails(IClock clock)
    {
        LastModifiedAt = clock.UtcNow;
        AddDomainEvent(new EntityModifiedEvent(Id, GetType().Name, clock.UtcNow));
    }
}

public record EntityModifiedEvent(object Id, string Name, DateTimeOffset ClockUtcNow) : IDomainEvent;

public record EntityCreatedEvent(object Id, string Name, DateTimeOffset ClockUtcNow) : IDomainEvent;