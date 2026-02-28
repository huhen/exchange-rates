using IdentityService.Api.Configurations;
using IdentityService.UseCases;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IdentityService.Api.Pipelines;

public class CachingBehavior<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger,
    IOptions<CachingOptions> cachingOptions
) : IPipelineBehavior<TRequest, TResponse?>
    where TRequest : IMessage
{
    private readonly CachingOptions _cachingOptions = cachingOptions.Value;

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
