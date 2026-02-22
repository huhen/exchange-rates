using ExchangeRates.SharedKernel;
using IdentityService.Core.UserAggregate.Events;

namespace IdentityService.Core.UserAggregate;

public sealed class User : EntityBase<User, UserId>, IAggregateRoot
{
    public UserName Name { get; private set; }
    public UserPassword Password { get; private set; }

    public User(UserName name, UserPassword password)
    {
        Name = name;
        Password = password;
    }

    // public User UpdatePassword(UserPassword newPassword)
    // {
    //     if (Password.Equals(newPassword)) return this;
    //     Password = newPassword;
    //     this.RegisterDomainEvent(new UserPasswordUpdatedEvent(this));
    //     return this;
    // }
}
