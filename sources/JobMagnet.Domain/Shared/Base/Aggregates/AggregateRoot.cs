using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Shared.Base.Aggregates;

public abstract class AggregateRoot<TId> : IHasIdentity<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TId Id { get; protected init; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    protected void ClearDomainEvents() => _domainEvents.Clear();
}