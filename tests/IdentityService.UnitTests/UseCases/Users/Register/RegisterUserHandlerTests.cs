using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Users.Register;

namespace IdentityService.UnitTests.UseCases.Users.Register;

public class RegisterUserHandlerTests
{
    private readonly UserName _testName = UserName.From("test-user");
    private readonly UserPassword _testPassword = UserPassword.From("Password1");
    private readonly IRepository<User> _repository = Substitute.For<IRepository<User>>();
    private readonly RegisterUserHandler _handler;

    public RegisterUserHandlerTests()
    {
        _handler = new RegisterUserHandler(_repository);
    }

    private User CreateUser()
    {
        return new User(_testName, _testPassword);
    }

    [Fact]
    public async Task ReturnsSuccessGivenValidNameAndPassword()
    {
        _repository.AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(CreateUser()));
        var result = await _handler.Handle(new RegisterUserCommand(_testName, _testPassword), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }
}
