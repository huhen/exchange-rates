namespace ExchangeRates.SharedKernel;

public class MediatorDomainEventDispatcher(
    IMediator mediator,
    ILogger<MediatorDomainEventDispatcher> logger
) : IDomainEventDispatcher
{
    public async Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents> entitiesWithEvents)
    {
        foreach (IHasDomainEvents entity in entitiesWithEvents)
        {
            if (entity is { } hasDomainEvents)
            {
                IDomainEvent[] events = hasDomainEvents.DomainEvents.ToArray();
                hasDomainEvents.ClearDomainEvents();

                foreach (var domainEvent in events)
                    await mediator.Publish(domainEvent).ConfigureAwait(false);
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
