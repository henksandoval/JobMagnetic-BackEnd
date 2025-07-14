using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Shared.Base.Entities;

public abstract class TrackableEntity<TId> : BaseEntity<TId>, ITrackable<TId>
{
    protected TrackableEntity()
    {
    }

    protected TrackableEntity(TId id) : base(id)
    {
    }

    public DateTimeOffset AddedAt { get; private set; }
    public DateTimeOffset? LastModifiedAt { get; private set; }

    internal void SetCreationTime(IClock clock)
    {
        AddedAt = clock.UtcNow;
    }

    internal void SetModificationTime(IClock clock)
    {
        LastModifiedAt = clock.UtcNow;
    }
}