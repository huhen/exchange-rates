using Microsoft.Extensions.Caching.Hybrid;

namespace IdentityService.Infrastructure.Caching;

public class CachingOptions
{
    public const string SectionName = "Caching";

    public int MaximumKeyLength { get; set; } = 512;
    public long MaximumPayloadBytes { get; set; } = 1024 * 1024 * 10;

    public HybridCacheEntryOptions DefaultEntryOptions { get; set; } = new()
    {
        Expiration = TimeSpan.FromMinutes(30), LocalCacheExpiration = TimeSpan.FromMinutes(1)
    };

    public Dictionary<string, HybridCacheEntryOptions?>? CacheProfiles { get; set; }
}
