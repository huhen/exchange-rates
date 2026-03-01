using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityService.Core.UserAggregate;

namespace IdentityService.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static UserId GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId)
            ? UserId.From(parsedUserId)
            : throw new ApplicationException("User id is unavailable");
    }

    public static string? GetJwtJti(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
    }

    public static long? GetJwtExp(this ClaimsPrincipal? principal)
    {
        string? exp = principal?.FindFirstValue(JwtRegisteredClaimNames.Exp);

        return long.TryParse(exp, out long parsedExp) ? parsedExp : null;
    }
}
