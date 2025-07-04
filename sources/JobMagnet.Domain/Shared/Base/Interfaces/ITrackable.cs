namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface ITrackable<out TId> : IHasIdentity<TId>, IAuditableEntity;