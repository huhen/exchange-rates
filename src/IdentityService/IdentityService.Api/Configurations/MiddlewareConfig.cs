using IdentityService.Infrastructure.Authentication;
using IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Api.Configurations;

public static class MiddlewareConfig
{
    public static async Task<IApplicationBuilder> UseAppMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.MapEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Use global exception handler in both dev and prod
        app.UseExceptionHandler();

        app.MapAuthentications();
        
        if (app.Environment.IsDevelopment())
        {
            await SeedDatabase(app);
        }
        
        app.MapDefaultEndpoints();
        
        return app;
    }

    static async Task SeedDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
            await SeedData.InitializeAsync(context,services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB. {ExceptionMessage}", ex.Message);
        }
    }
}
