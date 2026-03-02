using IdentityService.Infrastructure.Authentication;
using IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Api.Configurations;

public static class MiddlewareConfig
{
    /// <summary>
    /// Configures application middleware and endpoint mappings, enabling request logging, global exception handling, authentication, and (in development) Swagger and database seeding.
    /// </summary>
    /// <param name="app">The web application to configure.</param>
    /// <returns>The configured <see cref="IApplicationBuilder"/> instance.</returns>
    public static async Task<IApplicationBuilder> UseAppMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.MapAuthentications();
        
        app.MapEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Use global exception handler in both dev and prod
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            await SeedDatabase(app);
        }

        app.MapDefaultEndpoints();

        return app;
    }

    /// <summary>
    /// Applies pending EF Core migrations and initializes seed data using a scoped service provider, logging any errors that occur.
    /// </summary>
    /// <param name="app">The WebApplication whose service provider is used to create the scope for obtaining the database context and other services.</param>
    static async Task SeedDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
            await SeedData.InitializeAsync(context, services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB. {ExceptionMessage}", ex.Message);
        }
    }
}
