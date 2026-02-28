using IdentityService.Core.UserAggregate.Events;

namespace IdentityService.Core.UserAggregate;

public sealed class User : EntityBase<User, UserId>, IAggregateRoot
{
    public UserName Name { get; private set; }
    public UserPasswordHash PasswordHash { get; private set; }

    public User(UserName name, UserPasswordHash passwordHash)
    {
        Name = name;
        PasswordHash = passwordHash;
        
        RegisterDomainEvent(new UserRegisteredEvent(this));
    }
}
