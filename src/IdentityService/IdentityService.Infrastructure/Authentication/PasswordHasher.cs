using System.Security.Cryptography;
using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;

namespace IdentityService.Infrastructure.Authentication;

public sealed class PasswordHasher(ILogger<PasswordHasher> logger) : IPasswordHasher
{
    private const int KeySize = 256 / 8;
    private const int SaltSize = 128 / 8;
    private const int Iterations = 100_000;

    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

    /// <summary>
    /// Produces a salted password hash suitable for storage by combining a cryptographically generated salt and a derived key into a single Base64 string.
    /// </summary>
    /// <param name="password">The plaintext password to hash.</param>
    /// <returns>A <see cref="UserPasswordHash"/> whose Value is the Base64 encoding of: the 16-byte salt followed by the 32-byte derived key produced with PBKDF2 (SHA-512, 100000 iterations).</returns>
    public UserPasswordHash Hash(UserPassword password)
    {
        Span<byte> hashedPasswordBytes = stackalloc byte[SaltSize + KeySize];
        var saltBytes = hashedPasswordBytes[..SaltSize];
        var keyBytes = hashedPasswordBytes[SaltSize..];

        RandomNumberGenerator.Fill(saltBytes);
        Rfc2898DeriveBytes.Pbkdf2(password.Value, saltBytes, keyBytes, Iterations, _algorithm);

        return UserPasswordHash.From(Convert.ToBase64String(hashedPasswordBytes));
    }

    /// <summary>
    /// Verifies that the provided plaintext password matches the stored password hash.
    /// </summary>
    /// <param name="password">The plaintext password to verify.</param>
    /// <param name="passwordHash">The stored combined salt+key encoded as Base64.</param>
    /// <returns>`true` if the password matches the stored hash, `false` otherwise.</returns>
    /// <remarks>
    /// If <paramref name="passwordHash"/> is not a valid Base64-encoded salt+key, the method logs an error and returns `false`.
    /// </remarks>
    public bool Verify(UserPassword password, UserPasswordHash passwordHash)
    {
        Span<byte> hashedPasswordBytes = stackalloc byte[SaltSize + KeySize];
        if (!Convert.TryFromBase64String(passwordHash.Value, hashedPasswordBytes, out _))
        {
            logger.LogError("Invalid password hash: {PasswordHash}", passwordHash);
            return false;
        }

        var saltBytes = hashedPasswordBytes[..SaltSize];
        var expectedKeyBytes = hashedPasswordBytes[SaltSize..];

        Span<byte> actualKeyBytes = stackalloc byte[KeySize];
        Rfc2898DeriveBytes.Pbkdf2(password.Value, saltBytes, actualKeyBytes, Iterations, _algorithm);

        return CryptographicOperations.FixedTimeEquals(expectedKeyBytes, actualKeyBytes);
    }
}
