using EntityFramework.Exceptions.PostgreSQL;
using IdentityService.Infrastructure.Authentication;
using IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        string environmentName)
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

        services.AddAuthenticationConfig(configuration);

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }

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

    private static void RegisterEfRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    }
}
