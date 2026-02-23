using System.Reflection;
using IdentityService.Api.Configurations;
using IdentityService.Api.Extensions;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var environmentName = builder.Environment.EnvironmentName;

    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
    
    builder.AddLoggerConfigs();
    
    // add (domain)handlers

    builder.Services.AddPresentationConfigs();
    
    // add infrastructure

    builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

    var app = builder.Build();
    
    app.UseSerilogRequestLogging();

    app.MapEndpoints();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    // use middlewares
    app.UseExceptionHandler();

    // app.UseAuthentication();
    // app.UseAuthorization();

    await app.RunAsync();

    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program;
