using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Shared.Base.Entities;

public abstract class BaseEntity<TId> : IHasIdentity<TId>
{
    protected BaseEntity()
    {
    }

    protected BaseEntity(TId id)
    {
        Id = id;
    }

    public TId Id { get; protected init; }
}