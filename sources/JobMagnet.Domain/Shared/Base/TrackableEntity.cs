using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Shared.Base;

public abstract class TrackableEntity<TId>(TId id, Guid addedBy) : ITrackableEntity<TId>
{
    public TId Id { get; protected set; } = id;
    public Guid AddedBy { get; protected set; } = addedBy;
    public DateTime AddedAt { get; protected set; } = DateTime.UtcNow;
    public Guid? LastModifiedBy { get; protected set; }
    public DateTime? LastModifiedAt { get; protected set; }

    protected virtual void UpdateModificationDetails(Guid? modifiedBy)
    {
        LastModifiedBy = modifiedBy;
        LastModifiedAt = DateTime.UtcNow;
    }
}