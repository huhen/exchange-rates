using CSharpFunctionalExtensions;
using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users.Login;

public record LoginUserCommand(UserName Name, UserPassword Password) : ICommand<Result<UserId>>;
