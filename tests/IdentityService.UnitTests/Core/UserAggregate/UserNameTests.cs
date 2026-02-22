namespace IdentityService.UnitTests.Core.UserAggregate;

public class UserNameTests
{
    [Theory]
    [InlineData("ab")]           // Минимальная длина
    [InlineData("abc")]
    [InlineData("a-b")]
    [InlineData("a.b")]
    [InlineData("test-user")]
    [InlineData("test.user")]
    [InlineData("a.b.c-d-e")]
    [InlineData("abcdefghijklmnopqrstuvwxabcdefghijklmnopqrstuvwxabcdefghijkl")] // 63 символа (максимум)
    public void From_WhenValidName_ReturnsSuccess(string input)
    {
        var userName = UserName.From(input);

        userName.Value.ShouldBe(input);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void From_WhenNullOrWhitespace_ThrowsException(string? input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserName.From(input!));
    }

    [Theory]
    [InlineData("a")] // 1 символ (меньше минимума)
    public void From_WhenTooShort_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserName.From(input));
    }

    [Theory]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // 64 символа (ошибка, максимум 63)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // 65 символов
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // 100 символов
    public void From_WhenTooLong_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserName.From(input));
    }

    [Theory]
    [InlineData("Abc")]      // Заглавная буква
    [InlineData("ABC")]      // Все заглавные
    [InlineData("test_user")] // Подчёркивание
    [InlineData("test user")] // Пробел
    [InlineData("test@user")] // Спецсимвол @
    [InlineData("test#user")] // Спецсимвол #
    [InlineData("test$user")] // Спецсимвол $
    [InlineData("test!user")] // Спецсимвол !
    [InlineData("тест")]      // Кириллица
    [InlineData("テスト")]     // Японские иероглифы
    public void From_WhenInvalidCharacters_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserName.From(input));
    }

    [Fact]
    public void Equality_WhenSameValue_ReturnsTrue()
    {
        var userName1 = UserName.From("test-user");
        var userName2 = UserName.From("test-user");

        userName1.ShouldBe(userName2);
        userName1.GetHashCode().ShouldBe(userName2.GetHashCode());
    }

    [Fact]
    public void Equality_WhenDifferentValue_ReturnsFalse()
    {
        var userName1 = UserName.From("test-user.a");
        var userName2 = UserName.From("test-user.b");

        userName1.ShouldNotBe(userName2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var userName = UserName.From("test-user");

        userName.ToString().ShouldBe("test-user");
    }
}
