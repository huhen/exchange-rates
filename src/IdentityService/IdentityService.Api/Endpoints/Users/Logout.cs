using System.IdentityModel.Tokens.Jwt;
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
            .RequireAuthorization();
    }

    private static async Task<Results<Ok, ProblemHttpResult>> ExecuteAsync(
        ILogger<Logout> logger,
        HttpContext ctx,
        // IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            var jti = ctx.User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
            var exp = long.Parse(ctx.User.FindFirst(JwtRegisteredClaimNames.Exp)!.Value);

            await Task.Delay(100, cancellationToken);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении состояния модема");
            return TypedResults.Problem(detail: "Внутренняя ошибка сервера", statusCode: 500);
        }
    }
}
