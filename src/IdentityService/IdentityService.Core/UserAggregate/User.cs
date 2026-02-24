namespace IdentityService.Core.UserAggregate;

public sealed class User : EntityBase<User, UserId>, IAggregateRoot
{
    public UserName Name { get; private set; }
    // TODO: Need use password hash for security
    public UserPassword Password { get; private set; }

    public User(UserName name, UserPassword password)
    {
        Name = name;
        Password = password;
    }

    public bool VerifyPassword(UserPassword password) => Password.Equals(password);
}
