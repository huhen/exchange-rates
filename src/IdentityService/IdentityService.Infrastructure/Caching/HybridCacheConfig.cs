using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure.Caching;

public static class HybridCacheConfig
{
    /// <summary>
    /// Configures caching-related services by binding CachingOptions, registering HybridCache, and conditionally adding Redis and its health check.
    /// </summary>
    /// <param name="instanceName">Instance name applied to the Redis cache entries when a Redis connection string is present.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    internal static IServiceCollection AddHybridCacheConfig(this IServiceCollection services,
        IConfiguration configuration, string instanceName)
    {
        services.Configure<CachingOptions>(configuration.GetSection(CachingOptions.SectionName));

        var cachingOptions = new CachingOptions();
        configuration
            .GetSection(CachingOptions.SectionName)
            .Bind(cachingOptions);

        // Add Redis
        var connectionString = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = instanceName;
            });

            services.AddHealthChecks().AddRedis(connectionString);
        }

        // Add HybridCache - it will automatically use Redis as L2
        services.AddHybridCache(options =>
        {
            // Maximum size of cached items
            options.MaximumPayloadBytes = cachingOptions.MaximumPayloadBytes;
            options.MaximumKeyLength = cachingOptions.MaximumKeyLength;

            // Default timeouts
            options.DefaultEntryOptions = cachingOptions.DefaultEntryOptions;
        });

        return services;
    }
}
