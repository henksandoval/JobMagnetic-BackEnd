namespace JobMagnet.Domain.Shared.Base;

public abstract class BaseAggregate<TId>(TId id)
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public TId Id { get; protected init; } = id;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    protected void ClearDomainEvents() => _domainEvents.Clear();
}