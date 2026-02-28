using System.Reflection;
using IdentityService.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IdentityService.Api.Configurations;

public static class PresentationConfigs
{
    internal static IServiceCollection AddPresentationConfig(this IServiceCollection services,
        IConfiguration configuration, Assembly assembly)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services.AddEndpoints(assembly);
    }

    private static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    internal static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}
