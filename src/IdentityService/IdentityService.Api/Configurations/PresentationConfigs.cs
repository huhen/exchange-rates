using System.Reflection;
using IdentityService.Api.Endpoints;
using IdentityService.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;

namespace IdentityService.Api.Configurations;

public static class PresentationConfigs
{
    /// <summary>
    /// Configure API explorer and Swagger with a JWT Bearer security definition, then register endpoint implementations discovered in the provided assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for concrete IEndpoint implementations to register with the service collection.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with Swagger setup and endpoint services added.</returns>
    internal static IServiceCollection AddPresentationConfig(this IServiceCollection services, Assembly assembly)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "JWT Bearer token"
                });

            options.OperationFilter<SecurityRequirementsOperationFilter>(JwtBearerDefaults.AuthenticationScheme);
        });

        return services.AddEndpoints(assembly);
    }

    /// <summary>
    /// Scans the provided assembly for concrete types that implement <c>IEndpoint</c> and registers each as a transient <c>IEndpoint</c> service.
    /// </summary>
    /// <param name="services">The service collection to register endpoint services into.</param>
    /// <param name="assembly">The assembly to scan for concrete <c>IEndpoint</c> implementations.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance with discovered endpoint services added.</returns>
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

    /// <summary>
    /// Maps all registered IEndpoint implementations into the application's routing by invoking each endpoint's MapEndpoint method.
    /// </summary>
    /// <param name="app">The WebApplication whose service provider will be used to resolve registered endpoints and whose routing will be returned.</param>
    /// <param name="routeGroupBuilder">Optional route group to register endpoints on; when null, endpoints are mapped directly on the application.</param>
    /// <returns>The original <see cref="WebApplication"/> instance.</returns>
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

    /// <summary>
    /// Applies an authorization policy requirement to the route using the specified policy name.
    /// </summary>
    /// <param name="permission">The name of the authorization policy to require for the route.</param>
    /// <returns>The same <see cref="RouteHandlerBuilder"/> configured with the specified authorization requirement.</returns>
    public static RouteHandlerBuilder HasPermission(this RouteHandlerBuilder app, string permission)
    {
        return app.RequireAuthorization(permission);
    }
}
