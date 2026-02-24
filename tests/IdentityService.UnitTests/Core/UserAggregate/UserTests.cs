using IdentityService.Core.UserAggregate;

namespace IdentityService.UnitTests.Core.UserAggregate;

public class UserTests
{
    [Fact]
    public void Constructor_SetsNameAndPassword()
    {
        var name = UserName.From("test-user");
        var password = UserPassword.From("Password1");

        var user = new User(name, password);

        user.Name.ShouldBe(name);
        user.Password.ShouldBe(password);
    }

    [Fact]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        var name = UserName.From("test-user");
        var password = UserPassword.From("Password1");
        var user = new User(name, password);

        var result = user.VerifyPassword(password);

        result.ShouldBeTrue();
    }

    [Fact]
    public void VerifyPassword_IncorrectPassword_ReturnsFalse()
    {
        var name = UserName.From("test-user");
        var password = UserPassword.From("Password1");
        var wrongPassword = UserPassword.From("WrongPassword2");
        var user = new User(name, password);

        var result = user.VerifyPassword(wrongPassword);

        result.ShouldBeFalse();
    }
}
