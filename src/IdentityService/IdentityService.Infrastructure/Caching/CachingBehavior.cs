using IdentityService.UseCases;
using Mediator;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Caching;

public class CachingBehavior<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger,
    IOptions<CachingOptions> cachingOptions
) : IPipelineBehavior<TRequest, TResponse?>
    where TRequest : IMessage
{
    private readonly CachingOptions _cachingOptions = cachingOptions.Value;

    /// <summary>
    /// Applies caching for requests that implement <see cref="ICacheable"/> by returning a cached response when available; otherwise invokes the next handler and caches its result according to the resolved cache profile.
    /// </summary>
    /// <param name="request">The incoming request; must implement <see cref="ICacheable"/> to participate in caching.</param>
    /// <param name="next">Delegate to invoke the downstream handler to produce the response when a cache miss occurs.</param>
    /// <param name="cancellationToken">Token to observe while retrieving or creating the cached entry.</param>
    /// <returns>`TResponse?` containing the cached response for the request, or the downstream handler's result when no cached entry exists.</returns>
    public async ValueTask<TResponse?> Handle(
        TRequest request,
        MessageHandlerDelegate<TRequest, TResponse?> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheable cacheable)
            return await next(request, cancellationToken);

        HybridCacheEntryOptions? cacheOptions = null;

        if (cacheable.CacheProfile is { } cacheProfileName)
        {
            _cachingOptions.CacheProfiles?.TryGetValue(cacheProfileName, out cacheOptions);
        }

        var cacheKey = cacheable.GetCacheKey();

        return await cache.GetOrCreateAsync(cacheKey, async token =>
        {
            logger.LogDebug("Cache miss. Getting data from database {CacheKey}", cacheKey);
            return await next(request, token);
        }, options: cacheOptions, cancellationToken: cancellationToken);
    }
}
