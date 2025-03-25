namespace JobMagnet.Infrastructure.Entities.Base.Interfaces;

public interface ISoftDeletableEntity<TId> : IHasIdentity<TId>, IAuditableEntity
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }
}