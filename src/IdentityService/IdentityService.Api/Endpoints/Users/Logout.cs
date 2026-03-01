using IdentityService.Api.Configurations;
using IdentityService.Core.UserAggregate;
using IdentityService.Infrastructure.Authorization;
using IdentityService.UseCases.Abstractions.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IdentityService.Api.Endpoints.Users;

internal sealed class Logout : IEndpoint
{
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
