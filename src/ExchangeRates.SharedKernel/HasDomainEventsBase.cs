namespace ExchangeRates.SharedKernel;

public abstract class HasDomainEventsBase : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    [NotMapped] public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Registers a domain event on this instance so it is included in the DomainEvents collection until cleared.
    /// </summary>
    /// <param name="domainEvent">The domain event to record for this instance.</param>
    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Removes all domain events currently tracked by this instance.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}
