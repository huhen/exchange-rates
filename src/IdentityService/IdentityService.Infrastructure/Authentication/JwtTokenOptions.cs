namespace IdentityService.Infrastructure.Authentication;

public class JwtTokenOptions
{
    public const string SectionName = "JwtToken";

    public const string JwksEndpoint = "/.well-known/jwks";
    public const string OpenIdConfigurationEndpoint = "/.well-known/openid-configuration";

    public string CertificatePath { get; set; } = string.Empty;
    public string CertificatePassword { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 1;
    public string Audience { get; set; } = "internal-services";
    public string Issuer { get; set; } = "http://identity-service";
    public string OpenIdConfigurationUri { get; set; } = "http://identity-service:5000" + OpenIdConfigurationEndpoint;
    public string JwksUri { get; set; } = "http://identity-service:5000" + JwksEndpoint;
    public bool RequireHttpsMetadata { get; set; } = false;
}
