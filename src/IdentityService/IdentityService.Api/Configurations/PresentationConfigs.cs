using IdentityService.Api.Infrastructure;

namespace IdentityService.Api.Configurations;

public static class PresentationConfigs
{
    internal static IServiceCollection AddPresentationConfigs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
