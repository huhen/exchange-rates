using IdentityService.Core.UserAggregate.Events;
using Microsoft.Extensions.Caching.Hybrid;

namespace IdentityService.UseCases.Users.GetPasswordHash;

public class UserRegisteredEventHandler(HybridCache cache)
    : INotificationHandler<UserRegisteredEvent>
{
    public ValueTask Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        return cache.RemoveAsync(new GetPasswordHashQuery(notification.User.Name).GetCacheKey(), cancellationToken);
    }
}
