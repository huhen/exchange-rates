using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Configurations;

internal sealed class GlobalExceptionHandler(
    IHostEnvironment env,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Handles an unhandled exception by logging it and writing a JSON ProblemDetails 500 response to the current HTTP context.
    /// </summary>
    /// <param name="httpContext">The current HTTP context whose response will be written.</param>
    /// <param name="exception">The exception to handle and include (development only) in the response detail.</param>
    /// <param name="cancellationToken">Cancellation token used when writing the response.</param>
    /// <returns>`true` indicating the exception was handled.</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        if (httpContext.Response.HasStarted)
        {
            logger.LogWarning("Response already started; global exception handler skipped.");
            return false;
        }
        
        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "Internal Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = env.IsDevelopment() ? exception.Message : "An error occurred while processing your request",
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
