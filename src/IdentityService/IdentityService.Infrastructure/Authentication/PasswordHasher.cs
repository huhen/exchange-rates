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

    public UserPasswordHash Hash(UserPassword password)
    {
        Span<byte> hashedPasswordBytes = stackalloc byte[SaltSize + KeySize];
        var saltBytes = hashedPasswordBytes[..SaltSize];
        var keyBytes = hashedPasswordBytes[SaltSize..];

        RandomNumberGenerator.Fill(saltBytes);
        Rfc2898DeriveBytes.Pbkdf2(password.Value, saltBytes, keyBytes, Iterations, _algorithm);

        return UserPasswordHash.From(Convert.ToBase64String(hashedPasswordBytes));
    }

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
