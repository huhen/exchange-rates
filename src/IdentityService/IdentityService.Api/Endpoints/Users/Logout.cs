using IdentityService.Api.Configurations;
using IdentityService.Core.UserAggregate;
using IdentityService.Infrastructure.Authorization;
using IdentityService.UseCases.Abstractions.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IdentityService.Api.Endpoints.Users;

internal sealed class Logout : IEndpoint
{
    /// <summary>
    /// Registers the POST "/users/logout" endpoint and configures its metadata, authorization, permission, and response types.
    /// </summary>
    /// <param name="app">The endpoint route builder used to add the logout route.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/logout", ExecuteAsync)
            .WithSummary("Logout user")
            .WithDescription("Logout and revoke refresh token.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Users)
            .HasPermission(Permissions.UsersAccess)
            .RequireAuthorization();
    }

    /// <summary>
    /// Handles a logout request for the current user.
    /// </summary>
    /// <remarks>
    /// Currently returns 200 OK and does not revoke refresh tokens (TODO: revoke refresh token).
    /// </remarks>
    /// <returns>An HTTP 200 OK result indicating the logout endpoint was handled.</returns>
    private static Ok ExecuteAsync(
        ILogger<Logout> logger,
        // IUserContext userContext,
        // IMediator mediator,
        CancellationToken cancellationToken)
    {
        // TODO: Revoke refresh token

        return TypedResults.Ok();
    }
}
