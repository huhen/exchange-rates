namespace IdentityService.UnitTests.Core.UserAggregate;

public class UserPasswordTests
{
    [Theory]
    [InlineData("12345678")]                          // Минимальная длина
    [InlineData("Password1")]
    [InlineData("MyP@ssw0rd")]
    [InlineData("Test!@#$%^&*()")]
    [InlineData("abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_+-=[]{}|;:',./<>?")] // 64 символа (максимум)
    public void From_WhenValidPassword_ReturnsSuccess(string input)
    {
        var userPassword = UserPassword.From(input);

        userPassword.Value.ShouldBe(input);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void From_WhenNullOrWhitespace_ThrowsException(string? input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPassword.From(input!));
    }

    [Theory]
    [InlineData("1234567")] // 7 символов (меньше минимума)
    [InlineData("abcd")]    // 4 символа
    public void From_WhenTooShort_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPassword.From(input));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_+-=[]{}|;:',./<>?1")] // 65 символов
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // 100 символов
    public void From_WhenTooLong_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPassword.From(input));
    }

    [Theory]
    [InlineData("password ")]  // Пробел в конце
    [InlineData(" pass word")] // Пробелы внутри
    [InlineData("pass\tword")] // Табуляция
    [InlineData("pass\nword")] // Новая строка
    [InlineData("pass\rword")] // Возврат каретки
    [InlineData("пароль123")]  // Кириллица (не ASCII)
    [InlineData("パスワード")]   // Японские иероглифы (не ASCII)
    [InlineData("passéword")]  // Расширенный ASCII (не printable ASCII 0x21-0x7E)
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
