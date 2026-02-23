using CSharpFunctionalExtensions;
using ExchangeRates.SharedKernel;
using IdentityService.Core.UserAggregate;
using IdentityService.Core.UserAggregate.Specifications;

namespace IdentityService.UseCases.Users.Login;

public class LoginUserHandler(IReadRepository<User> repository)
    : ICommandHandler<LoginUserCommand, Result<UserId>>
{
    private const string ErrorMessage = "Incorrect login/password";

    public async ValueTask<Result<UserId>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var spec = new UserByNameSpec(command.Name);
        var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity is null || entity.Password != command.Password)
            return Result.Failure<UserId>(ErrorMessage);

        return Result.Success(entity.Id);
    }
}
