using IdentityService.Core.UserAggregate.Events;
using Microsoft.Extensions.Caching.Hybrid;

namespace IdentityService.UseCases.Users.GetPasswordHash;

public class UserRegisteredEventHandler(HybridCache cache)
    : INotificationHandler<UserRegisteredEvent>
{
    /// <summary>
    /// Removes the cached password-hash entry for the newly registered user.
    /// </summary>
    /// <param name="notification">The event containing the newly registered user's details.</param>
    /// <param name="cancellationToken">Token to cancel the cache removal operation.</param>
    /// <returns>A ValueTask that completes when the cache entry removal has finished.</returns>
    public ValueTask Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        return cache.RemoveAsync(new GetPasswordHashQuery(notification.User.Name).GetCacheKey(), cancellationToken);
    }
}
