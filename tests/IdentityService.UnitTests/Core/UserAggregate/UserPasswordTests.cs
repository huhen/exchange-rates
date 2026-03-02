using IdentityService.Core.UserAggregate;

namespace IdentityService.UnitTests.Core.UserAggregate;

public class UserPasswordTests
{
    [Theory]
    [InlineData("12345678")]
    [InlineData("Password1")]
    [InlineData("MyP@ssw0rd")]
    [InlineData("Test!@#$%^&*()")]
    [InlineData("abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_+-=[]{}|;:',./<>?")]
    [InlineData("pass word")]
    [InlineData(" pass word ")]
    [InlineData("my password 123")]
    public void From_WhenValidPassword_ReturnsSuccess(string input)
    {
        var userPassword = UserPassword.From(input);

        userPassword.Value.ShouldBe(input);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")] // 8 пробелов
    public void From_WhenNullOrWhitespace_ThrowsException(string? input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPassword.From(input!));
    }

    [Theory]
    [InlineData("1234567")] // 7 символов (меньше минимума)
    [InlineData("abcd")] // 4 символа
    public void From_WhenTooShort_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPassword.From(input));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_+-=[]{}|;:',./<>?1")] // 65 символов
    [InlineData("abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_+-=[]{}|;:',./<>?12")] // 66 символов
    public void From_WhenTooLong_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPassword.From(input));
    }

    [Theory]
    [InlineData("pass\tword")]
    [InlineData("pass\nword")]
    [InlineData("pass\rword")]
    [InlineData("пароль123")]
    public void From_WhenInvalidCharacters_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPassword.From(input));
    }

    [Fact]
    public void Equality_WhenSameValue_ReturnsTrue()
    {
        var password1 = UserPassword.From("Password1");
        var password2 = UserPassword.From("Password1");

        password1.ShouldBe(password2);
        password1.GetHashCode().ShouldBe(password2.GetHashCode());
    }

    [Fact]
    public void Equality_WhenDifferentValue_ReturnsFalse()
    {
        var password1 = UserPassword.From("Password1");
        var password2 = UserPassword.From("Password2");

        password1.ShouldNotBe(password2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var password = UserPassword.From("Password1");

        password.ToString().ShouldBe("Password1");
    }
}
