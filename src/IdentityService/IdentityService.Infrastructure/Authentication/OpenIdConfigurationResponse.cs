using System.Text.Json.Serialization;

namespace IdentityService.Infrastructure.Authentication;

// public sealed record OpenIdConfigurationResponse(
//     string Issuer, // "https://github.com"
//     string JwksUri, // "https://github.com/login/oauth/.well-known/jwks"
//     string[] SubjectTypesSupported, // ["public"]
//     string[] ResponseTypesSupported, // ["code","id_token"]
//     string[] ClaimsSupported, // ["sub","aud","exp","nbf","iat","iss","act"]
//     string[] IdTokenSigningAlgValuesSupported, // ["RS256"]
//     string[] ScopesSupported // ["openid"]
// );

public sealed class OpenIdConfigurationResponse
{
    [JsonPropertyName("issuer")] public required string Issuer { get; init; }

    [JsonPropertyName("jwks_uri")] public required string JwksUri { get; init; }

    [JsonPropertyName("subject_types_supported")]
    public required string[] SubjectTypesSupported { get; init; }

    [JsonPropertyName("response_types_supported")]
    public required string[] ResponseTypesSupported { get; init; }

    [JsonPropertyName("claims_supported")] public required string[] ClaimsSupported { get; init; }

    [JsonPropertyName("id_token_signing_alg_values_supported")]
    public required string[] IdTokenSigningAlgValuesSupported { get; init; }

    [JsonPropertyName("scopes_supported")] public required string[]? ScopesSupported { get; init; }
}
