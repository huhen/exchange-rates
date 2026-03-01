using System.Diagnostics;
using System.Reflection;

namespace IdentityService.Api.Pipelines;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    /// <summary>
    /// Logs request handling (Information and optional per-property Trace), measures handling time, invokes the next pipeline delegate, and returns its response.
    /// </summary>
    /// <param name="request">The request message being handled and logged.</param>
    /// <param name="next">Delegate that produces the response by continuing pipeline execution.</param>
    /// <param name="cancellationToken">Token to observe for cancellation when invoking <paramref name="next"/>.</param>
    /// <returns>The response produced by the next pipeline handler.</returns>
    public async ValueTask<TResponse> Handle(
        TRequest request,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!logger.IsEnabled(LogLevel.Information))
            return await next(request, cancellationToken);

        logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

        if (logger.IsEnabled(LogLevel.Trace))
        {
            Type myType = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                object? propValue = prop.GetValue(request, null);
                logger.LogTrace("Property {Property} : {@Value}", prop.Name, propValue);
            }
        }

        var startingTimestamp = Stopwatch.GetTimestamp();

        var response = await next(request, cancellationToken);

        var elapsedMilliseconds = Stopwatch.GetElapsedTime(startingTimestamp).TotalMilliseconds;

        logger.LogInformation("Handled {RequestName} with {Response} in {ms:F3} ms",
            typeof(TRequest).Name,
            response,
            elapsedMilliseconds);

        return response;
    }
}
