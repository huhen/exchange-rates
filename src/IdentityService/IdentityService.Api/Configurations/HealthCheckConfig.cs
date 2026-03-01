using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IdentityService.Api.Configurations;

internal static  class HealthCheckConfig
{
    internal const string HealthEndpointPath = "/health";
    internal const string AlivenessEndpointPath = "/alive";
    
    /// <summary>
    /// Registers a default set of health checks on the provided host builder.
    /// </summary>
    /// <typeparam name="TBuilder">The host builder type that implements <see cref="IHostApplicationBuilder"/>.</typeparam>
    /// <param name="builder">The host application builder to configure with default health checks.</param>
    /// <returns>The same builder instance to allow fluent chaining.</returns>
    internal static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    /// <summary>
    /// Maps default health-check endpoints onto the application when running in the Development environment.
    /// </summary>
    /// <returns>The same <see cref="WebApplication"/> instance with health-check endpoints mapped when applicable.</returns>
    /// <remarks>
    /// When the application's environment is Development, this maps:
    /// - <c>/health</c> to expose all registered health checks.
    /// - <c>/alive</c> to expose only health checks tagged with <c>live</c>.
    /// These mappings are not applied in non-Development environments.
    /// </remarks>
    internal static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks(HealthEndpointPath);

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks(AlivenessEndpointPath, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }

        return app;
    }
}
