using JobMagnet.Domain.Core.Entities.Base.Interfaces;

namespace JobMagnet.Domain.Core.Entities.Base;

public abstract class TrackableEntity<TId> : ITrackableEntity<TId>
{
    public required TId Id { get; set; }
    public Guid AddedBy { get; set; }
    public DateTime AddedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}