using JobMagnet.Infrastructure.Entities.Base.Interfaces;

namespace JobMagnet.Infrastructure.Entities.Base;

public abstract class TrackableEntity<TId> : ITrackableEntity<TId>
{
    public required TId Id { get; set; }
    public required Guid AddedBy { get; set; }
    public required DateTime AddedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}