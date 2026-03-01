using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Abstractions.Authentication;

public interface IUserContext
{
    UserId UserId { get; }
}
