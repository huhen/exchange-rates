using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Abstractions.Authentication;

public interface ITokenProvider
{
    /// <summary>
/// Creates a signed authentication token for the specified user.
/// </summary>
/// <param name="userId">Identifier of the user to issue the token for.</param>
/// <returns>The generated authentication token as a string.</returns>
string Create(UserId userId);
}
