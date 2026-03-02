using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Infrastructure.Authentication;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public UserId UserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new UserContextUnavailableException();
}
