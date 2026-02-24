using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users.Register;

public record RegisterUserCommand(UserName Name, UserPassword Password) : ICommand<Result<UserId>>;
