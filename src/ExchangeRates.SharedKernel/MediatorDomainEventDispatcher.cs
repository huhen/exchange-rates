namespace ExchangeRates.SharedKernel;

public class MediatorDomainEventDispatcher(
    IMediator mediator,
    ILogger<MediatorDomainEventDispatcher> logger
) : IDomainEventDispatcher
{
    /// <summary>
    /// Publishes each domain event from the provided entities and clears those events from each entity after capturing them.
    /// </summary>
    /// <param name="entitiesWithEvents">A collection of entities to process; for each entity that implements <see cref="IHasDomainEvents"/>, its domain events are published and then cleared.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents> entitiesWithEvents, CancellationToken cancellationToken)
    {
        foreach (IHasDomainEvents entity in entitiesWithEvents)
        {
            if (entity is { } hasDomainEvents)
            {
                IDomainEvent[] events = hasDomainEvents.DomainEvents.ToArray();
                hasDomainEvents.ClearDomainEvents();

                foreach (var domainEvent in events)
                    await mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                logger.LogError(
                    "Entity of type {EntityType} does not inherit from {BaseType}. Unable to clear domain events.",
                    entity.GetType().Name,
                    nameof(IHasDomainEvents));
            }
        }
    }
}
