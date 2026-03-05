using CurrencyService.Core.CurrencyAggregate;
using Vogen;

namespace CurrencyService.UnitTests.Core.CurrencyAggregate;

public class CurrencyRateTests
{
    [Theory]
    [InlineData(1.0)]
    [InlineData(0.5)]
    [InlineData(100.25)]
    public void From_WithValidRate_ShouldCreateSuccessfully(double validRate)
    {
        // Act
        var rate = CurrencyRate.From(validRate);

        // Assert
        rate.Value.ShouldBe(validRate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1.0)]
    [InlineData(-100.0)]
    [InlineData(double.NaN)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(double.PositiveInfinity)]
    public void From_WithInvalidRate_ShouldThrowValidationException(double invalidRate)
    {
        // Act & Assert
        var exception = Should.Throw<ValueObjectValidationException>(() => CurrencyRate.From(invalidRate));
        exception.Message.ShouldContain("must be a positive finite number");
    }
}
