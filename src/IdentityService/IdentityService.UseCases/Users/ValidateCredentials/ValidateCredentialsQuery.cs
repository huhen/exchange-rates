using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users.ValidateCredentials;

public record ValidateCredentialsQuery(UserName Name, UserPassword Password) : IQuery<Result>, ICacheable
{
    public string? CacheProfile => null;

    public string GetCacheKey()
    {
        // TODO: Need use password hash for security
        return $"{nameof(ValidateCredentialsQuery)}-{Name.Value}-{Password.Value}";
    }
}
