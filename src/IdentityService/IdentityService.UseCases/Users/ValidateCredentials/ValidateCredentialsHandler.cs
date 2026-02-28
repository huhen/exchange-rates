using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;
using IdentityService.UseCases.Users.GetPasswordHash;

namespace IdentityService.UseCases.Users.ValidateCredentials;

public class ValidateCredentialsHandler(
    IMediator mediator,
    IPasswordHasher passwordHasher
) : IQueryHandler<ValidateCredentialsQuery, Result<UserId>>
{
    private const string ErrorMessage = "Incorrect login/password";

    public async ValueTask<Result<UserId>> Handle(ValidateCredentialsQuery request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetPasswordHashQuery(request.Name), cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<UserId>(ErrorMessage);

        return Result.Success(user.Id);
    }
}
