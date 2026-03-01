namespace IdentityService.Core;

public static class CoreServiceExtensions
{
    /// <summary>
    /// Registers core project services into the provided service collection and logs an informational message indicating core services were registered.
    /// </summary>
    /// <returns>The same <see cref="IServiceCollection"/> instance passed in to allow method chaining.</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services, ILogger logger)
    {
        logger.LogInformation("{Project} services registered", "Core");

        return services;
    }
}
