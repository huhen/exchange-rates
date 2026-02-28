using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityService.Core.UserAggregate;
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

    public TokenProvider(IOptions<JwtTokenOptions> options)
    {
        _settings = options.Value;

        using var certificate = new X509Certificate2(
            _settings.CertificatePath,
            _settings.CertificatePassword,
            X509KeyStorageFlags.EphemeralKeySet);

        using var ecdsa = certificate.GetECDsaPrivateKey();
        var securityKey = new ECDsaSecurityKey(ecdsa)
        {
            KeyId = certificate.Thumbprint,
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = true }
        };

        _credentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);
        
        _handler = new JsonWebTokenHandler();
    }

    public string Create(UserId userId)
    {
        var now = DateTime.UtcNow;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
