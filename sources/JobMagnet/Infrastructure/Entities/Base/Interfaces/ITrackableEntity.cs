namespace JobMagnet.Infrastructure.Entities.Base.Interfaces;

public interface ITrackableEntity<TId> : IHasIdentity<TId>, IAuditableEntity;