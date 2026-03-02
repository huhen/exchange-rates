using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Abstractions.Authentication;

public interface IPasswordHasher
{
    /// <summary>
    /// Produces a hash representation of the specified user password for storage and later verification.
    /// </summary>
    /// <param name="password">The user's password to be hashed.</param>
    /// <returns>A <see cref="UserPasswordHash"/> containing the hashed form of the provided password.</returns>
    UserPasswordHash Hash(UserPassword password);

    /// <summary>
    /// Determines whether the provided user password matches the given password hash.
    /// </summary>
    /// <param name="password">The user password to verify.</param>
    /// <param name="passwordHash">The stored password hash to compare against.</param>
    /// <returns>`true` if the password matches the hash, `false` otherwise.</returns>
    bool Verify(UserPassword password, UserPasswordHash passwordHash);
}
