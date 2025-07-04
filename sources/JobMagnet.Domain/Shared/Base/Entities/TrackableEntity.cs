using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Shared.Base.Entities;

public abstract class TrackableEntity<TId> : BaseEntity<TId>, ITrackable<TId>
{
    public DateTimeOffset AddedAt { get; private set; }
    public DateTimeOffset? LastModifiedAt { get; private set; }

    protected TrackableEntity() : base() {}

    protected TrackableEntity(TId id) : base(id)
    {
    }

    internal void SetCreationTime(IClock clock)
    {
        AddedAt = clock.UtcNow;
    }

    internal void SetModificationTime(IClock clock)
    {
        LastModifiedAt = clock.UtcNow;
    }
}