namespace JobMagnet.Domain.Entities.Base.Interfaces;

public interface ITrackableEntity<TId> : IHasIdentity<TId>, IAuditableEntity;