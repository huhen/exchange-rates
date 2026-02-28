using IdentityService.Api.Helpers;
using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Users.Register;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed record RegisterRequest(string UserName, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", ExecuteAsync)
            .WithSummary("Register a new user")
            .WithDescription($"""
                              Register a new user with the provided name and password.
                              The user name must be between {UserName.MinLength} and {UserName.MaxLength} characters long.
                              The password must be between {UserPassword.MinLength} and {UserPassword.MaxLength} characters long.
                              """)
            .Accepts<RegisterRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Users);
    }

    private static async ValueTask<Results<Created, ValidationProblem, Conflict<ProblemDetails>>> ExecuteAsync(
        RegisterRequest request,
        ILogger<Register> logger,
        HttpContext ctx,
        IMediator mediator,
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

        var command = new RegisterUserCommand(userName.ValueObject, userPassword.ValueObject);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return TypedResults.Conflict(new ProblemDetails { Detail = result.Error });

        return TypedResults.Created();
    }
}
