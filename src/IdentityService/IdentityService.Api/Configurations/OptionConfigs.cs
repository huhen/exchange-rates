namespace IdentityService.Api.Configurations;

public static class OptionConfigs
{
    /// <summary>
    /// Configures cookie policy options, registers a global exception handler and Problem Details support, and returns the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="logger">Logger used to record an informational message about the configuration.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance with the option configurations applied.</returns>
    public static IServiceCollection AddOptionConfigs(this IServiceCollection services,
        Microsoft.Extensions.Logging.ILogger logger)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        // Register global exception handler
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        logger.LogInformation("{Project} were configured", "Configuration and Options");

        return services;
    }
}
