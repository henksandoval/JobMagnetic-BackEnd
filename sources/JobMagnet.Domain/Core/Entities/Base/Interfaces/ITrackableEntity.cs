namespace JobMagnet.Domain.Core.Entities.Base.Interfaces;

public interface ITrackableEntity<TId> : IHasIdentity<TId>, IAuditableEntity;