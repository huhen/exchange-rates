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
        
        return TypedResults.Ok(new LoginResponse(accessToken));
    }
}
