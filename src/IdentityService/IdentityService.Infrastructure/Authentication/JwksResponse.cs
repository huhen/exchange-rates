using System.Text.Json.Serialization;

namespace IdentityService.Infrastructure.Authentication;

public sealed record JwksResponse
{
    [JsonPropertyName("keys")] public required List<JwksKeyDto> Keys { get; init; }
}

public sealed record JwksKeyDto
{
    [JsonPropertyName("kty")] public required string Kty { get; init; }
    [JsonPropertyName("alg")] public required string Alg { get; init; }
    [JsonPropertyName("crv")] public required string Crv { get; init; }
    [JsonPropertyName("use")] public required string Use { get; init; }
    [JsonPropertyName("kid")] public required string Kid { get; init; }
    [JsonPropertyName("x")] public required string X { get; init; }
    [JsonPropertyName("y")] public required string Y { get; init; }
}
