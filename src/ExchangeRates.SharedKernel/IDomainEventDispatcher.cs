namespace ExchangeRates.SharedKernel;

public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches domain events found on the provided entities and clears those events from the entities afterwards.
    /// </summary>
    /// <param name="entitiesWithEvents">A collection of entities that implement <see cref="IHasDomainEvents"/> whose domain events should be dispatched and then cleared.</param>
    /// <returns>A <see cref="Task"/> representing the operation of dispatching all domain events and clearing them from the supplied entities.</returns>
    Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents> entitiesWithEvents);
}
