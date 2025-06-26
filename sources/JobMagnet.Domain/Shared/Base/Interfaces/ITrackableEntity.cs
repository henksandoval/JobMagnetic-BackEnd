namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface ITrackableEntity<TId> : IHasIdentity<TId>, IAuditableEntity;