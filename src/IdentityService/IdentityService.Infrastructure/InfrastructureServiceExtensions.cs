using EntityFramework.Exceptions.PostgreSQL;
using IdentityService.Infrastructure.Authentication;
using IdentityService.Infrastructure.Caching;
using IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure;

public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Registers infrastructure services into the DI container, including repository wiring, caching, and authentication; conditionally configures the application's DbContext when the environment is not "Testing".
    /// </summary>
    /// <param name="environmentName">The current environment name; when this equals "Testing", the database context registration is skipped.</param>
    /// <param name="applicationName">The application name used to scope or name infrastructure registrations such as hybrid caching.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        string environmentName,
        string applicationName)
    {
        if (environmentName != "Testing")
        {
            AddDbContextWithNpgsql(services, configuration);
            // services.AddScoped<IListUsersQueryService, ListUsersQueryService>();
        }
        // else
        // {
        //     services.AddScoped<IListUsersQueryService, FakeListUsersQueryService>();
        // }

        RegisterEfRepositories(services);

        services.AddHybridCacheConfig(configuration, applicationName);

        services.AddAuthenticationConfig(configuration);

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }

    /// <summary>
    /// Registers the application's Entity Framework Core DbContext and related infrastructure when a "Database" connection string is present.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    /// <param name="configuration">The application configuration used to read the "Database" connection string.</param>
    private static void AddDbContextWithNpgsql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        services.AddScoped<EventDispatchInterceptor>();
        services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(provider.GetRequiredService<EventDispatchInterceptor>())
                .UseExceptionProcessor();
        });
        // services.AddDbContextPool<AppDbContext>((provider, options) =>
        // {
        //     options
        //         .UseNpgsql(connectionString, npgsqlOptions =>
        //             npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
        //         .UseSnakeCaseNamingConvention()
        //         .AddInterceptors(provider.GetRequiredService<EventDispatchInterceptor>())
        //         .UseExceptionProcessor();
        // });

        services.AddHealthChecks().AddNpgSql(connectionString);
    }

    /// <summary>
    /// Registers generic Entity Framework repository implementations with the provided service collection.
    /// </summary>
    /// <remarks>
    /// Maps IRepository&lt;T&gt; and IReadRepository&lt;T&gt; to EfRepository&lt;T&gt; using a scoped lifetime.
    /// </remarks>
    private static void RegisterEfRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    }
}
