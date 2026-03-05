namespace CurrencyService.Core.CurrencyAggregate.Specifications;

public class CurrencyByCharCodeSpec : Specification<Currency>, ISingleResultSpecification<Currency>
{
    /// <summary>
    /// Creates a specification that filters currencies to those whose CharCode equals the provided value.
    /// </summary>
    /// <param name="charCode">The currency character code used to filter the specification.</param>
    public CurrencyByCharCodeSpec(CurrencyCharCode charCode)
    {
        Query.Where(currency => currency.CharCode == charCode);
    }
}
