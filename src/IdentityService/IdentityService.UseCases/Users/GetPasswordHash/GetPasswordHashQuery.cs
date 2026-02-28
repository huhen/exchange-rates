using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users.GetPasswordHash;

public record GetPasswordHashQuery(UserName Name) : IQuery<UserIdAndHashDto?>, ICacheable
{
    public string CacheProfile => nameof(GetPasswordHashQuery);

    public string GetCacheKey()
    {
        return $"{nameof(GetPasswordHashQuery)}-{Name.Value}";
    }
}
