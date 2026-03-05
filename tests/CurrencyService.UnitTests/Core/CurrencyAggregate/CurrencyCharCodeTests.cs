using CurrencyService.Core.CurrencyAggregate;
using Vogen;

namespace CurrencyService.UnitTests.Core.CurrencyAggregate;

public class CurrencyCharCodeTests
{
    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("RUB")]
    public void From_WithValidCode_ShouldCreateSuccessfully(string validCode)
    {
        // Act
        var charCode = CurrencyCharCode.From(validCode);

        // Assert
        charCode.Value.ShouldBe(validCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("usd")]
    [InlineData("US")]
    [InlineData("USDA")]
    [InlineData("US1")]
    [InlineData("123")]
    public void From_WithInvalidCode_ShouldThrowValidationException(string invalidCode)
    {
        // Act & Assert
        var exception = Should.Throw<ValueObjectValidationException>(() => CurrencyCharCode.From(invalidCode));
        exception.Message.ShouldContain("exactly 3 uppercase letters");
    }
}
