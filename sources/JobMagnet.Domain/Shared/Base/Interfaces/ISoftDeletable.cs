namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface ISoftDeletable<out TId> : IHasIdentity<TId>, IAuditableEntity
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedAt { get; }
}