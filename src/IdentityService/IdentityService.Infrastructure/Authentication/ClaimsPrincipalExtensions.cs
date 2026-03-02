using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityService.Core.UserAggregate;

namespace IdentityService.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Retrieves the user's identifier from the NameIdentifier claim of the provided principal.
    /// </summary>
    /// <param name="principal">The claims principal containing authentication claims; may be null.</param>
    /// <returns>The UserId represented by the NameIdentifier claim if is present and parsable, `null` otherwise.</returns>
    public static UserId? GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId)
            ? UserId.From(parsedUserId)
            : null;
    }

    /// <summary>
    /// Gets the JWT ID ("jti") claim value from the provided claims principal.
    /// </summary>
    /// <param name="principal">The claims principal to read the claim from.</param>
    /// <returns>The JTI claim value, or null if the claim is not present.</returns>
    public static string? GetJwtJti(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
    }

    /// <summary>
    /// Retrieves the JWT expiration time ("exp" claim) from the principal's claims.
    /// </summary>
    /// <param name="principal">The claims principal that may contain the JWT claims; may be null.</param>
    /// <returns>The expiration time as a Unix timestamp (`long`) if the "exp" claim is present and parsable, `null` otherwise.</returns>
    public static long? GetJwtExp(this ClaimsPrincipal? principal)
    {
        string? exp = principal?.FindFirstValue(JwtRegisteredClaimNames.Exp);

        return long.TryParse(exp, out long parsedExp) ? parsedExp : null;
    }
}
