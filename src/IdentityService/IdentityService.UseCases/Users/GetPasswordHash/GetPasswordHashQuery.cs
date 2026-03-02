using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users.GetPasswordHash;

public record GetPasswordHashQuery(UserName Name) : IQuery<UserIdAndHashDto?>, ICacheable
{
    public string CacheProfile => nameof(GetPasswordHashQuery);

    /// <summary>
    /// Produces the cache key for this query by combining the query type name with the wrapped user name value.
    /// </summary>
    /// <returns>The cache key in the format "GetPasswordHashQuery-{userNameValue}".</returns>
    public string GetCacheKey()
    {
        return $"{nameof(GetPasswordHashQuery)}-{Name.Value}";
    }
}
