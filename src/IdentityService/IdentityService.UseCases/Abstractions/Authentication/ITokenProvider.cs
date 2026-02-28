using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(UserId userId);
}
