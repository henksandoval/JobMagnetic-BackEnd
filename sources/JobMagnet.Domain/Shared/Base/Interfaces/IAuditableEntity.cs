namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface IAuditableEntity
{
    Guid AddedBy { get; }
    DateTime AddedAt { get; }
    Guid? LastModifiedBy { get; }
    DateTime? LastModifiedAt { get; }
}