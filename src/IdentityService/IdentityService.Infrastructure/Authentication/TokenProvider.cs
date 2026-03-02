using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityService.Core.UserAggregate;
using IdentityService.Infrastructure.Authorization;
using IdentityService.UseCases.Abstractions.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Authentication;

public sealed class TokenProvider : ITokenProvider
{
    private readonly JwtTokenOptions _settings;
    private readonly SigningCredentials _credentials;
    private readonly JsonWebTokenHandler _handler;

    /// <summary>
    /// Initializes the token provider from configured JWT options and prepares the ECDSA signing credentials and JSON web token handler.
    /// </summary>
    /// <param name="options">An <see cref="IOptions{JwtTokenOptions}"/> containing issuer, audience, expiration and certificate settings used to create the signing key.</param>
    public TokenProvider(IOptions<JwtTokenOptions> options)
    {
        _settings = options.Value;

        if (string.IsNullOrWhiteSpace(_settings.CertificatePath))
            throw new InvalidOperationException("JWT certificate path is not configured.");
        if (_settings.ExpirationInMinutes <= 0)
            throw new InvalidOperationException("JWT expiration must be greater than zero.");

        using var certificate = new X509Certificate2(
            _settings.CertificatePath,
            _settings.CertificatePassword);

        var ecdsa = certificate.GetECDsaPrivateKey()
                    ?? throw new InvalidOperationException("JWT certificate must contain an ECDSA private key.");

        var securityKey = new ECDsaSecurityKey(ecdsa)
        {
            KeyId = certificate.Thumbprint,
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = true }
        };

        _credentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);

        _handler = new JsonWebTokenHandler();
    }

    /// <summary>
    /// Creates a JSON Web Token containing the user's subject, a unique token id, and a role claim granting user access.
    /// </summary>
    /// <param name="userId">The identifier of the user to include as the token subject.</param>
    /// <returns>A JWT as a compact serialized string.</returns>
    public string Create(UserId userId)
    {
        var now = DateTime.UtcNow;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Permissions.JwtRoleClaimName, Permissions.UsersAccess)
            ]),
            IssuedAt = now,
            Expires = now.AddMinutes(_settings.ExpirationInMinutes),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = _credentials
        };

        return _handler.CreateToken(tokenDescriptor);
    }
}
