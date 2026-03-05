using CurrencyService.Core.CurrencyAggregate;

namespace CurrencyService.UnitTests.Core.CurrencyAggregate;

public class CurrencyTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        var charCode = CurrencyCharCode.From("USD");
        var name = CurrencyName.From("US Dollar");

        // Act
        var currency = new Currency(charCode, name);

        // Assert
        currency.CharCode.ShouldBe(charCode);
        currency.Name.ShouldBe(name);
    }

    [Fact]
    public void UpdateName_WithNewName_ShouldUpdateName()
    {
        // Arrange
        var currency = new Currency(CurrencyCharCode.From("USD"), CurrencyName.From("US Dollar"));
        var newName = CurrencyName.From("United States Dollar");

        // Act
        var result = currency.UpdateName(newName);

        // Assert
        result.ShouldBeSameAs(currency);
        currency.Name.ShouldBe(newName);
    }

    [Fact]
    public void UpdateName_WithSameName_ShouldNotModifyAndReturnSame()
    {
        // Arrange
        var initialName = CurrencyName.From("US Dollar");
        var currency = new Currency(CurrencyCharCode.From("USD"), initialName);

        // Act
        var result = currency.UpdateName(initialName);

        // Assert
        result.ShouldBeSameAs(currency);
        currency.Name.ShouldBe(initialName);
    }

    [Fact]
    public void UpdateRate_WithNewRate_ShouldUpdateRate()
    {
        // Arrange
        var currency = new Currency(CurrencyCharCode.From("USD"), CurrencyName.From("US Dollar"));
        var newRate = CurrencyRate.From(1.5);

        // Act
        var result = currency.UpdateRate(newRate);

        // Assert
        result.ShouldBeSameAs(currency);
        currency.Rate.ShouldBe(newRate);
    }

    [Fact]
    public void UpdateRate_WithSameRate_ShouldNotModifyAndReturnSame()
    {
        // Arrange
        var currency = new Currency(CurrencyCharCode.From("USD"), CurrencyName.From("US Dollar"));
        var initialRate = CurrencyRate.From(1.5);
        currency.UpdateRate(initialRate);

        // Act
        var result = currency.UpdateRate(initialRate);

        // Assert
        result.ShouldBeSameAs(currency);
        currency.Rate.ShouldBe(initialRate);
    }
}
