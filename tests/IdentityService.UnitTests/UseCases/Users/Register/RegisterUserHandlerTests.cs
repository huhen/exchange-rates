using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;
using IdentityService.UseCases.Users.Register;

namespace IdentityService.UnitTests.UseCases.Users.Register;

public class RegisterUserHandlerTests
{
    private readonly IRepository<User> _repository = Substitute.For<IRepository<User>>();
    private readonly IPasswordHasher _passwordHasher = new FakePasswordHasher();
    private readonly RegisterUserHandler _handler;

    public RegisterUserHandlerTests()
    {
        _handler = new RegisterUserHandler(_repository, _passwordHasher);
    }

    [Fact]
    public async Task ReturnsSuccessWithUserIdGivenValidInput()
    {
        var userName = UserName.From("new-user");
        var userPassword = UserPassword.From("Password1");
        var passwordHash = _passwordHasher.Hash(userPassword);
        var user = new User(userName, passwordHash) { Id = UserId.From(Guid.NewGuid()) };

        _repository.AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);

        var result = await _handler.Handle(new RegisterUserCommand(userName, userPassword), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(user.Id);
    }

    [Fact]
    public async Task ReturnsFailureGivenDuplicateUser()
    {
        var userName = UserName.From("existing-user");
        var userPassword = UserPassword.From("Password1");

        _repository
            .When(x => x.AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new EntityFramework.Exceptions.Common.UniqueConstraintException());

        var result = await _handler.Handle(new RegisterUserCommand(userName, userPassword), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldContain("already exists");
    }
}
