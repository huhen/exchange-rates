using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;

namespace IdentityService.UnitTests;

public class FakePasswordHasher : IPasswordHasher
{
    public UserPasswordHash Hash(UserPassword password)
    {
        return UserPasswordHash.From($"HASHED_{password.Value}");
    }

    public bool Verify(UserPassword password, UserPasswordHash passwordHash)
    {
        return passwordHash.Value == $"HASHED_{password.Value}";
    }
}
