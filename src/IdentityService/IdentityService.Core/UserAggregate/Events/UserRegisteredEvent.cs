namespace IdentityService.Core.UserAggregate.Events;

public sealed class UserRegisteredEvent(User user) : DomainEventBase
{
    public User User { get; private set; } = user;
}
