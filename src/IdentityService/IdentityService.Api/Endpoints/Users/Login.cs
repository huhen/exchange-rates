using IdentityService.Api.Helpers;
using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;
using IdentityService.UseCases.Users.ValidateCredentials;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IdentityService.Api.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record LoginRequest(string UserName, string Password);

    public sealed record LoginResponse(string AccessToken);

    /// <summary>
    /// Registers the POST /users/login endpoint on the provided route builder and configures its request and response metadata.
    /// </summary>
    /// <param name="app">The endpoint route builder to add the login endpoint to.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", ExecuteAsync)
            .WithSummary("Login user")
            .WithDescription("Authenticate user with username and password.")
            .Accepts<LoginRequest>("application/json")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Users);
    }

    /// <summary>
    /// Handle a login request by validating the supplied credentials and issuing an access token when authentication succeeds.
    /// </summary>
    /// <param name="request">Incoming login payload containing the user name and password.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>`Ok&lt;LoginResponse&gt;` containing an access token on successful authentication; `ValidationProblem` when input validation fails; `ProblemHttpResult` carrying a 401 Unauthorized detail for invalid credentials or other problem details for server errors.</returns>
    private static async ValueTask<Results<Ok<LoginResponse>, ValidationProblem, ProblemHttpResult>> ExecuteAsync(
        LoginRequest request,
        ILogger<Login> logger,
        IMediator mediator,
        ITokenProvider tokenProvider,
        CancellationToken cancellationToken)
    {
        var userName = UserName.TryFrom(request.UserName);
        var userPassword = UserPassword.TryFrom(request.Password);

        if (VogenValidationHelper.Validate(
                userName, nameof(request.UserName),
                userPassword, nameof(request.Password))
            is { } validationProblem)
        {
            return validationProblem;
        }

        var command = new ValidateCredentialsQuery(userName.ValueObject, userPassword.ValueObject);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return TypedResults.Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                detail: result.Error
            );

        var accessToken = tokenProvider.Create(result.Value);

        // TODO: Add refresh token

        return TypedResults.Ok(new LoginResponse(accessToken));
    }
}
