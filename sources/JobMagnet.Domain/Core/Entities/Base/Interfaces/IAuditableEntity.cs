namespace JobMagnet.Domain.Core.Entities.Base.Interfaces;

public interface IAuditableEntity
{
    Guid AddedBy { get; set; }
    DateTime AddedAt { get; set; }
    Guid? LastModifiedBy { get; set; }
    DateTime? LastModifiedAt { get; set; }
}