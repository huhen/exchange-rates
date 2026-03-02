using IdentityService.Core.UserAggregate.Events;

namespace IdentityService.Core.UserAggregate;

public sealed class User : EntityBase<User, UserId>, IAggregateRoot
{
    public UserName Name { get; private set; }
    public UserPasswordHash PasswordHash { get; private set; }

    /// <summary>
    /// Creates a new User with the specified name and password hash and registers a UserRegisteredEvent.
    /// </summary>
    /// <param name="name">The user's name.</param>
    /// <param name="passwordHash">The user's password hash.</param>
    public User(UserName name, UserPasswordHash passwordHash)
    {
        Name = name;
        PasswordHash = passwordHash;

        RegisterDomainEvent(new UserRegisteredEvent(this));
    }
}
