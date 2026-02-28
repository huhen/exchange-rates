namespace IdentityService.Api.Configurations;

public static class HybridCacheConfig
{
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
