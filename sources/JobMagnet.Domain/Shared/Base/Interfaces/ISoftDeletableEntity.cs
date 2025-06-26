namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface ISoftDeletableEntity<TId> : IHasIdentity<TId>, IAuditableEntity
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    Guid? DeletedBy { get; }
}