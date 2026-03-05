namespace CurrencyService.Core.CurrencyAggregate;

[ValueObject<decimal>]
public partial struct CurrencyRate
{
    private static Validation Validate(decimal value)
        => value > 0
            ? Validation.Ok
            : Validation.Invalid($"{nameof(CurrencyRate)} must be a positive finite number");
}
