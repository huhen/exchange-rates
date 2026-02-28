using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Abstractions.Authentication;

public interface IPasswordHasher
{
    UserPasswordHash Hash(UserPassword password);

    bool Verify(UserPassword password, UserPasswordHash passwordHash);
}
