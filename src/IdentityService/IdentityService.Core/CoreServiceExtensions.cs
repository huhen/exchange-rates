using IdentityService.Core.Interfaces;
using IdentityService.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Core;

public static class CoreServiceExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, ILogger logger)
    {
        // services.AddScoped<IUpdateUserPasswordService, UpdateUserPasswordService>();

        logger.LogInformation("{Project} services registered", "Core");

        return services;
    }
}
