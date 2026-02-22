using Microsoft.AspNetCore.Http.HttpResults;

namespace IdentityService.Api.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed record RegisterRequest(string UserName, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", ExecuteAsync)
            .WithSummary("Register a new user")
            .WithDescription("""
                             Register a new user with the provided name and password.
                             The user name must be between 2 and 100 characters long.
                             The password must be between 8 and 64 characters long.
                             """)
            .Accepts<RegisterRequest>("application/json")
            .Produces<RegisterRequest>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Users);
    }

    private static async Task<Results<Created, ValidationProblem, ProblemHttpResult>> ExecuteAsync(
        RegisterRequest request,
        ILogger<Register> logger,
        HttpContext ctx,
        // IMediator mediator,
        CancellationToken cancellationToken)
    {
        // try
        // {
        await Task.Delay(100, cancellationToken);
        return TypedResults.Created();
        // }
        //     logger.LogError(ex, "An error occurred during user registration");
        //     return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
        // }
    }
}
