using IdentityService.Core.UserAggregate;

namespace IdentityService.UnitTests.Core.UserAggregate;

public class UserTests
{
    [Fact]
    public void Constructor_SetsNameAndPassword()
    {
        var name = UserName.From("test-user");
        var password = UserPasswordHash.From("Password1");

        var user = new User(name, password);

        user.Name.ShouldBe(name);
        user.PasswordHash.ShouldBe(password);
    }
}
