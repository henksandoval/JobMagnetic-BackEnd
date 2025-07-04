namespace JobMagnet.Domain.Shared.Base.Aggregates;

public abstract class BaseAggregate<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TId Id { get; protected init; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected BaseAggregate()
    {
    }

    protected BaseAggregate(TId id)
    {
        Id = id;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    protected void ClearDomainEvents() => _domainEvents.Clear();
}