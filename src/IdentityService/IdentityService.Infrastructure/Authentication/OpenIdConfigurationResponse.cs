using System.Text.Json.Serialization;

namespace IdentityService.Infrastructure.Authentication;

public sealed record OpenIdConfigurationResponse
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

    [JsonPropertyName("scopes_supported")] public required string[] ScopesSupported { get; init; }
};
