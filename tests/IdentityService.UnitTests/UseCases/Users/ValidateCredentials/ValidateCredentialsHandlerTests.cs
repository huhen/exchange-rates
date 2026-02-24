using IdentityService.Core.UserAggregate;
using IdentityService.Core.UserAggregate.Specifications;
using IdentityService.UseCases.Users.ValidateCredentials;

namespace IdentityService.UnitTests.UseCases.Users.ValidateCredentials;

public class ValidateCredentialsHandlerTests
{
    private readonly UserName _testName = UserName.From("test-user");
    private readonly UserPassword _testPassword = UserPassword.From("Password1");
    private readonly IReadRepository<User> _repository = Substitute.For<IReadRepository<User>>();
    private readonly ValidateCredentialsHandler _handler;

    public ValidateCredentialsHandlerTests()
    {
        _handler = new ValidateCredentialsHandler(_repository);
    }

    [Fact]
    public async Task ReturnsSuccessGivenValidCredentials()
    {
        var user = new User(_testName, _testPassword);
        _repository.FirstOrDefaultAsync(Arg.Any<UserByNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(user);
        var result = await _handler.Handle(new ValidateCredentialsQuery(_testName, _testPassword),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnsFailureGivenInvalidPassword()
    {
        var wrongPassword = UserPassword.From("WrongPassword2");
        var user = new User(_testName, _testPassword);
        _repository.FirstOrDefaultAsync(Arg.Any<UserByNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(user);
        var result = await _handler.Handle(new ValidateCredentialsQuery(_testName, wrongPassword),
            CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task ReturnsFailureGivenNonExistentUser()
    {
        _repository.FirstOrDefaultAsync(Arg.Any<UserByNameSpec>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        var result = await _handler.Handle(new ValidateCredentialsQuery(_testName, _testPassword),
            CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
    }
}
