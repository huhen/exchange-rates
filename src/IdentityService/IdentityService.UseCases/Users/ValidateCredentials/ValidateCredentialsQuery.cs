using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users.ValidateCredentials;

public record ValidateCredentialsQuery(UserName Name, UserPassword Password) : IQuery<Result<UserId>>;
