namespace CurrencyService.Core.CurrencyAggregate;

[ValueObject<double>]
public partial struct CurrencyRate
{
    private static Validation Validate(double value)
        => value > 0 && double.IsFinite(value) 
            ? Validation.Ok 
            : Validation.Invalid($"{nameof(CurrencyRate)} must be a positive finite number");
}
