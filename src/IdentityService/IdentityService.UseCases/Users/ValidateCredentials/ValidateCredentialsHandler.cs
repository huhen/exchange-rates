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
    
    private static readonly UserPasswordHash _dummyHash = UserPasswordHash.From("ExKjAszkL/GeM+fiteDc6v8MWvmllroXNRZOqKQFhguASuJnIxsCKTfNfpa+lfuk");
    
    /// <summary>
    /// Validates a user's credentials and returns the user's identifier on success.
    /// </summary>
    /// <param name="request">The query containing the user's name and password to validate.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>`Result&lt;UserId&gt;` that is successful with the user's Id when credentials match, or failed with the message "Incorrect login/password" when they do not.</returns>
    public async ValueTask<Result<UserId>> Handle(ValidateCredentialsQuery request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetPasswordHashQuery(request.Name), cancellationToken);

        var hashToVerify = user?.PasswordHash ?? _dummyHash;
        var isValidPassword = passwordHasher.Verify(request.Password, hashToVerify);
        
        if (user is null || !isValidPassword)
            return Result.Failure<UserId>(ErrorMessage);

        return Result.Success(user.Id);
    }
}
