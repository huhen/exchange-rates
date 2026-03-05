namespace CurrencyService.Core;

public static class CoreServiceExtensions
{
    /// <summary>
    /// Registers core framework services into the provided service collection and logs that core services were registered.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    /// <param name="logger">Logger used to emit an informational message about the registration.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance passed in to allow method chaining.</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services, ILogger logger)
    {
        logger.LogInformation("{Project} services registered", "Core");

        return services;
    }
}
