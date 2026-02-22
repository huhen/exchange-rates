using Microsoft.AspNetCore.Http.HttpResults;

namespace IdentityService.Api.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record LoginRequest(string UserName, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", ExecuteAsync)
            .WithSummary("Register a new user")
            .WithDescription("Register a new user with the provided name and password." +
                             "The user name must be between 2 and 100 characters long." +
                             "The password must be between 8 and 64 characters long.")
            .Accepts<LoginRequest>("application/json")
            .Produces<LoginRequest>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Users);
    }

    private static async Task<Results<Created, ValidationProblem, ProblemHttpResult>> ExecuteAsync(
        LoginRequest request,
        ILogger<Login> logger,
        // IMediator mediator,
        CancellationToken cancellationToken)
    {
        // try
        // {
            await Task.Delay(100, cancellationToken);
            
            if(request.UserName.Length < 2 || request.Password.Length < 8)
                throw new ApplicationException();
            
            return TypedResults.Created();
        // }
        // catch (Exception ex)
        // {
        //     logger.LogError(ex, "An unhandled exception occurred during {Operation}", nameof(Login));
        //     return TypedResults.Problem(detail: "Внутренняя ошибка сервера", statusCode: 500);
        // }
    }
}
