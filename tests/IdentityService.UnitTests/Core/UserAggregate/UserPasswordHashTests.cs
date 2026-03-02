using IdentityService.Core.UserAggregate;

namespace IdentityService.UnitTests.Core.UserAggregate;

public class UserPasswordHashTests
{
    [Theory]
    [InlineData("abc")]
    [InlineData("hash123")]
    [InlineData("HASHED_PASSWORD_123")]
    [InlineData("a")]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijk")] // 63 chars
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijkl")] // 64 chars
    public void From_WhenValidHash_ReturnsSuccess(string input)
    {
        var hash = UserPasswordHash.From(input);

        hash.Value.ShouldBe(input);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void From_WhenNullOrWhitespace_ThrowsException(string? input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPasswordHash.From(input!));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklm")] // 65 chars
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmn")] // 66 chars
    public void From_WhenTooLong_ThrowsException(string input)
    {
        Should.Throw<Vogen.ValueObjectValidationException>(() => UserPasswordHash.From(input));
    }
}
