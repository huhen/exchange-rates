namespace IdentityService.UseCases;

public interface ICacheable
{
    /// <summary>
    /// Produces the cache key that identifies this instance in caching operations.
    /// </summary>
    /// <returns>The cache key string used to store or retrieve this object from the cache.</returns>
    string GetCacheKey();

    string? CacheProfile { get; }
}
