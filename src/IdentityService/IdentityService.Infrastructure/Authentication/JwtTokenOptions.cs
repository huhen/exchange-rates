namespace IdentityService.Infrastructure.Authentication;

public class JwtTokenOptions
{
    public const string SectionName = "JwtToken";

    public string CertificatePath { get; set; } = string.Empty;
    public string CertificatePassword { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 1;
    public string Audience { get; set; } = "internal-services";
    public string Issuer { get; set; } = "identity-service";
    public string JwksEndpoint { get; set; } = "/.well-known/jwks.json";
}
