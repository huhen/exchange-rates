using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog.Enrichers.Span;
using Serilog.Sinks.OpenTelemetry;

namespace IdentityService.Api.Configurations;

internal static class LoggerConfigs
{
    /// <summary>
    /// Configures Serilog and OpenTelemetry integration on the provided WebApplicationBuilder.
    /// </summary>
    /// <returns>The same WebApplicationBuilder instance with Serilog and OpenTelemetry integration applied.</returns>
    internal static WebApplicationBuilder AddLoggerConfigs(this WebApplicationBuilder builder)
    {
        var serviceName = builder.Environment.ApplicationName;
        var instanceId = Environment.GetEnvironmentVariable("HOSTNAME") ?? Environment.MachineName;

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithSpan()
            .Enrich.WithProperty("Application", serviceName)
            .ConfigureOpenTelemetry(serviceName, instanceId)
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder.ConfigureOpenTelemetry(serviceName, instanceId);
    }

    /// <summary>
    /// Configures the provided Serilog LoggerConfiguration to export logs to an OpenTelemetry (OTLP) collector when an OTLP endpoint is specified via environment variables.
    /// </summary>
    /// <param name="loggerConfig">The LoggerConfiguration to augment.</param>
    /// <param name="serviceName">Value to set for the `service.name` resource attribute sent to the OTLP collector.</param>
    /// <param name="instanceId">Value to set for the `service.instance.id` resource attribute sent to the OTLP collector.</param>
    /// <returns>The original LoggerConfiguration with an OpenTelemetry sink configured when an OTLP endpoint is present; otherwise the original LoggerConfiguration unchanged.</returns>
    private static LoggerConfiguration ConfigureOpenTelemetry(this LoggerConfiguration loggerConfig, string serviceName,
        string instanceId)
    {
        var otelEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");
        if (string.IsNullOrWhiteSpace(otelEndpoint))
            return loggerConfig;

        var otelProtocolRaw = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_PROTOCOL")?.ToLower();
        var otelHeadersRaw = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_HEADERS");
        var otelResourceRaw = Environment.GetEnvironmentVariable("OTEL_RESOURCE_ATTRIBUTES");

        var otelProtocol = otelProtocolRaw == "http/protobuf" ? OtlpProtocol.HttpProtobuf : OtlpProtocol.Grpc;
        var parsedResourceAttributes = ParseKeyValueList(otelResourceRaw)
            .ToDictionary(k => k.Key, object (v) => v.Value);
        var parsedHeaders = ParseKeyValueList(otelHeadersRaw);

        parsedResourceAttributes["service.name"] = serviceName;
        parsedResourceAttributes["service.instance.id"] = instanceId;

        return loggerConfig.WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = otelEndpoint;
            options.Protocol = otelProtocol;
            options.ResourceAttributes = parsedResourceAttributes;
            options.Headers = parsedHeaders;
        });
    }

    /// <summary>
    /// Parses a comma-separated list of key=value pairs into a dictionary.
    /// </summary>
    /// <param name="input">A string containing comma-separated `key=value` pairs; keys and values may contain surrounding whitespace.</param>
    /// <returns>A dictionary mapping each parsed key to its corresponding trimmed value. Pairs without an '=' or with an empty key are ignored; when a key appears multiple times, the last value wins.</returns>
    private static Dictionary<string, string> ParseKeyValueList(string? input)
    {
        var dict = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(input))
            return dict;

        var pairs = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in pairs)
        {
            var kv = pair.Split('=', 2);
            if (kv.Length == 2)
            {
                var key = kv[0].Trim();
                var value = kv[1].Trim();
                if (!string.IsNullOrWhiteSpace(key))
                    dict[key] = value;
            }
        }

        return dict;
    }

    /// <summary>
    /// Registers OpenTelemetry tracing and metrics into the application's DI container and configures the service resource.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder to configure.</param>
    /// <param name="serviceName">The name to set for the OpenTelemetry service resource.</param>
    /// <param name="instanceId">The instance identifier to set for the OpenTelemetry service resource.</param>
    /// <returns>The same <see cref="WebApplicationBuilder"/> instance after OpenTelemetry services have been added.</returns>
    private static WebApplicationBuilder ConfigureOpenTelemetry(this WebApplicationBuilder builder, string serviceName,
        string instanceId)
    {
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(r =>
                r.AddService(
                    serviceName: serviceName,
                    serviceInstanceId: instanceId
                ))
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation(options =>
                        // Exclude health check requests from tracing
                        options.Filter = context =>
                            !context.Request.Path.StartsWithSegments(HealthCheckConfig.HealthEndpointPath)
                            && !context.Request.Path.StartsWithSegments(HealthCheckConfig.AlivenessEndpointPath)
                    )
                    .AddHttpClientInstrumentation()
                    .AddNpgsql()
                    .AddRedisInstrumentation()
                    .AddOtlpExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter();
            });

        return builder;
    }
}
