using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Shared.Base.Aggregates;

public abstract class TrackableAggregateRoot<TId> : AggregateRoot<TId>, ITrackable<TId>
{
    protected TrackableAggregateRoot()
    {
    }

    protected TrackableAggregateRoot(TId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt) : base(id)
    {
        AddedAt = addedAt;
        LastModifiedAt = lastModifiedAt;
    }

    protected TrackableAggregateRoot(TId id, IClock clock) : base(id)
    {
        AddedAt = clock.UtcNow;
        AddDomainEvent(new EntityCreatedEvent(Id, GetType().Name, clock.UtcNow));
    }

    public DateTimeOffset AddedAt { get; }
    public DateTimeOffset? LastModifiedAt { get; private set; }

    protected void UpdateModificationDetails(IClock clock)
    {
        LastModifiedAt = clock.UtcNow;
        AddDomainEvent(new EntityModifiedEvent(Id, GetType().Name, clock.UtcNow));
    }
}

public record EntityModifiedEvent(object Id, string Name, DateTimeOffset ClockUtcNow) : IDomainEvent;

public record EntityCreatedEvent(object Id, string Name, DateTimeOffset ClockUtcNow) : IDomainEvent;