using IdentityService.Core.UserAggregate;
using IdentityService.Core.UserAggregate.Specifications;
using IdentityService.UseCases.Users.GetPasswordHash;

namespace IdentityService.UnitTests.UseCases.Users.GetPasswordHash;

public class GetPasswordHashHandlerTests
{
    private readonly IReadRepository<User> _repository = Substitute.For<IReadRepository<User>>();
    private readonly GetPasswordHashHandler _handler;

    public GetPasswordHashHandlerTests()
    {
        _handler = new GetPasswordHashHandler(_repository);
    }

    [Fact]
    public async Task ReturnsUserIdAndHashGivenExistingUser()
    {
        var userName = UserName.From("test-user");
        var passwordHash = UserPasswordHash.From("hashed_password");
        var user = new User(userName, passwordHash) { Id = UserId.From(Guid.NewGuid()) };

        _repository.FirstOrDefaultAsync(Arg.Any<UserByNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<User?>(user));

        var result = await _handler.Handle(new GetPasswordHashQuery(userName), CancellationToken.None);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(user.Id);
        result.PasswordHash.ShouldBe(passwordHash);
    }

    [Fact]
    public async Task ReturnsNullGivenNonExistentUser()
    {
        var userName = UserName.From("nonexistent-user");

        _repository.FirstOrDefaultAsync(Arg.Any<UserByNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<User?>(null));

        var result = await _handler.Handle(new GetPasswordHashQuery(userName), CancellationToken.None);

        result.ShouldBeNull();
    }
}
