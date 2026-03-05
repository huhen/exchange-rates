using CurrencyService.Core.CurrencyAggregate;

namespace CurrencyService.UnitTests.Core.CurrencyAggregate;

public class CurrencyNameTests
{
    [Theory]
    [InlineData("US Dollar")]
    [InlineData("Euro")]
    [InlineData("Russian Ruble")]
    public void From_WithValidName_ShouldCreateSuccessfully(string validName)
    {
        // Act
        var name = CurrencyName.From(validName);

        // Assert
        name.Value.ShouldBe(validName);
    }

    [Fact]
    public void From_WithTooLongName_ShouldThrowValidationException()
    {
        // Arrange
        var invalidName = new string('A', 129);

        // Act & Assert
        var exception = Should.Throw<ValueObjectValidationException>(() => CurrencyName.From(invalidName));
        exception.Message.ShouldContain("no more than 128 characters");
    }
}
