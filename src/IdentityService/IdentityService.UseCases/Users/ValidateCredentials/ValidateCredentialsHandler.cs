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

    /// <summary>
    /// Validates a user's credentials and returns the user's identifier on success.
    /// </summary>
    /// <param name="request">The query containing the user's name and password to validate.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>`Result<UserId>` that is successful with the user's Id when credentials match, or failed with the message "Incorrect login/password" when they do not.</returns>
    public async ValueTask<Result<UserId>> Handle(ValidateCredentialsQuery request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetPasswordHashQuery(request.Name), cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<UserId>(ErrorMessage);

        return Result.Success(user.Id);
    }
}
