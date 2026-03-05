namespace CurrencyService.Core.CurrencyAggregate.Specifications;

public class CurrencyByCharCodeSpec : Specification<Currency>, ISingleResultSpecification<Currency>
{
    public CurrencyByCharCodeSpec(CurrencyCharCode charCode)
    {
        Query.Where(currency => currency.CharCode == charCode);
    }
}
