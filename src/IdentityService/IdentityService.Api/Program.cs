using System.Reflection;
using IdentityService.Api.Configurations;
using IdentityService.Core;
using IdentityService.Infrastructure;
using Microsoft.Extensions.Caching.Hybrid;
using Serilog.Extensions.Logging;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.WebHost.ConfigureKestrel(o =>
    {
        o.AddServerHeader = false; // <- removes "Server: Kestrel"
    });

    var environmentName = builder.Environment.EnvironmentName;

    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    builder.AddLoggerConfigs();

    builder.AddDefaultHealthChecks();

    builder.Services.ConfigureHttpClientDefaults(http =>
    {
        // Turn on resilience by default
        http.AddStandardResilienceHandler();
    });

    var appLogger = new SerilogLoggerFactory(Log.Logger).CreateLogger<Program>();

    builder.Services.AddOptionConfigs(appLogger);

    builder.Services.AddCoreServices(appLogger)
        .AddInfrastructureServices(builder.Configuration, appLogger, environmentName)
        .AddMediatorSourceGen(appLogger);

    builder.Services.AddPresentationConfig(builder.Configuration, Assembly.GetExecutingAssembly());

    builder.Services.AddHybridCacheConfig(builder.Configuration, builder.Environment.ApplicationName);

    var app = builder.Build();

    await app.UseAppMiddleware();

    await app.RunAsync();

    Log.Information("Stopped cleanly");
    return 0;
}
catch (HostAbortedException hostAbortedException)
{
    Log.Information(hostAbortedException,"Aborted");
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
