using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IdentityService.Infrastructure.Data;

// Intercepts SaveChanges to dispatch domain events after changes are successfully saved
public class EventDispatchInterceptor(IDomainEventDispatcher domainEventDispatcher) : SaveChangesInterceptor
{
    /// <summary>
    /// Dispatches and clears domain events from tracked entities after a successful SaveChangesAsync when the DbContext is an AppDbContext.
    /// </summary>
    /// <remarks>
    /// If the provided context is not an AppDbContext, this method delegates to the base implementation without dispatching events.
    /// </remarks>
    /// <param name="eventData">Contextual data for the completed SaveChanges operation, containing the DbContext.</param>
    /// <param name="result">The number of state entries written to the underlying database by the save operation.</param>
    /// <returns>The integer result from the base SavedChangesAsync call (number of state entries written).</returns>
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is not AppDbContext appDbContext)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);
        }

        // Retrieve all tracked entities that have domain events
        var entitiesWithEvents = appDbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        // Dispatch and clear domain events
        await domainEventDispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
