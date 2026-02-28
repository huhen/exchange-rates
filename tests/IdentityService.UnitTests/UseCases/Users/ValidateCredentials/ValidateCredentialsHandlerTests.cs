using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;
using IdentityService.UseCases.Users.GetPasswordHash;
using IdentityService.UseCases.Users.ValidateCredentials;
using IdentityService.UseCases.Users;

namespace IdentityService.UnitTests.UseCases.Users.ValidateCredentials;

public class ValidateCredentialsHandlerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IPasswordHasher _passwordHasher = new FakePasswordHasher();
    private readonly ValidateCredentialsHandler _handler;

    public ValidateCredentialsHandlerTests()
    {
        _handler = new ValidateCredentialsHandler(_mediator, _passwordHasher);
    }

    [Fact]
    public async Task ReturnsSuccessWithUserIdGivenValidCredentials()
    {
        var userName = UserName.From("test-user");
        var userPassword = UserPassword.From("Password1");
        var passwordHash = _passwordHasher.Hash(userPassword);
        var userId = UserId.From(Guid.NewGuid());

        _mediator.Send(Arg.Any<GetPasswordHashQuery>(), Arg.Any<CancellationToken>())
            .Returns(new UserIdAndHashDto(userId, passwordHash));

        var result = await _handler.Handle(new ValidateCredentialsQuery(userName, userPassword), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(userId);
    }

    [Fact]
    public async Task ReturnsFailureGivenInvalidPassword()
    {
        var userName = UserName.From("test-user");
        var correctPassword = UserPassword.From("CorrectPassword1");
        var wrongPassword = UserPassword.From("WrongPassword1");
        var passwordHash = _passwordHasher.Hash(correctPassword);
        var userId = UserId.From(Guid.NewGuid());

        _mediator.Send(Arg.Any<GetPasswordHashQuery>(), Arg.Any<CancellationToken>())
            .Returns(new UserIdAndHashDto(userId, passwordHash));

        var result = await _handler.Handle(new ValidateCredentialsQuery(userName, wrongPassword), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnsFailureGivenNonExistentUser()
    {
        var userName = UserName.From("nonexistent-user");
        var userPassword = UserPassword.From("Password1");

        _mediator.Send(Arg.Any<GetPasswordHashQuery>(), Arg.Any<CancellationToken>())
            .Returns((UserIdAndHashDto?)null);

        var result = await _handler.Handle(new ValidateCredentialsQuery(userName, userPassword), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
    }
}
