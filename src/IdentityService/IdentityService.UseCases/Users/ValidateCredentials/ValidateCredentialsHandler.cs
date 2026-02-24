using IdentityService.Core.UserAggregate;
using IdentityService.Core.UserAggregate.Specifications;

namespace IdentityService.UseCases.Users.ValidateCredentials;

public class ValidateCredentialsHandler(IReadRepository<User> repository)
    : IQueryHandler<ValidateCredentialsQuery, Result>
{
    private const string ErrorMessage = "Incorrect login/password";

    public async ValueTask<Result> Handle(ValidateCredentialsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserByNameSpec(request.Name);
        var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity is null || !entity.VerifyPassword(request.Password))
            return Result.Failure(ErrorMessage);

        return Result.Success();
    }
}
