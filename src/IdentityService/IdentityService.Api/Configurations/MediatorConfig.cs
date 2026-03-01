using IdentityService.Api.Pipelines;
using IdentityService.Core.UserAggregate;
using IdentityService.Infrastructure;
using IdentityService.Infrastructure.Caching;
using IdentityService.UseCases.Users.Register;

namespace IdentityService.Api.Configurations;

public static class MediatorConfig
{
    /// <summary>
    /// Registers MediatR Source Generator integration and pipeline behaviors into the provided service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="logger">Logger used to emit setup information.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance with mediator services and pipeline behaviors registered.</returns>
    public static IServiceCollection AddMediatorSourceGen(this IServiceCollection services,
        Microsoft.Extensions.Logging.ILogger logger)
    {
        logger.LogInformation("Registering Mediator SourceGen and Behaviors");
        services.AddMediator(options =>
        {
            // Lifetime: Singleton is fastest per docs; Scoped/Transient also supported.
            options.ServiceLifetime = ServiceLifetime.Scoped;

            // Supply any TYPE from each assembly you want scanned (the generator finds the assembly from the type)
            options.Assemblies =
            [
                typeof(User), // Core
                typeof(RegisterUserCommand), // UseCases
                typeof(InfrastructureServiceExtensions), // Infrastructure
                typeof(MediatorConfig) // Api
            ];

            // Register pipeline behaviors here (order matters)
            options.PipelineBehaviors =
            [
                typeof(LoggingBehavior<,>),
                typeof(CachingBehavior<,>)
            ];

            // If you have stream behaviors:
            // options.StreamPipelineBehaviors = [ typeof(YourStreamBehavior<,>) ];
        });

        // Alternative: register behaviors via DI yourself (useful if not doing AOT):
        // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        return services;
    }
}
