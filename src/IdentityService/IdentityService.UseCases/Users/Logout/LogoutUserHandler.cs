using CSharpFunctionalExtensions;
using ExchangeRates.SharedKernel;
using IdentityService.Core.UserAggregate;
using IdentityService.Core.UserAggregate.Specifications;

namespace IdentityService.UseCases.Users.Logout;

public class LogoutUserHandler(IReadRepository<User> repository)
    : ICommandHandler<LogoutUserCommand, Result>
{
    public async ValueTask<Result> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
    {
        var spec = new UserByIdSpec(command.UserId);
        var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        return entity is null ? Result.Failure("NotFound") : Result.Success();
    }
}
