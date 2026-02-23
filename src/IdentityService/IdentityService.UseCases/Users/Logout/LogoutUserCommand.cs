using CSharpFunctionalExtensions;
using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users.Logout;

public record LogoutUserCommand(UserId UserId) : ICommand<Result>;
