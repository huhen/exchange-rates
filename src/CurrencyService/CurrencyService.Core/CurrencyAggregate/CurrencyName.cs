namespace CurrencyService.Core.CurrencyAggregate;

[ValueObject<string>]
public partial struct CurrencyName
{
    public const int MaxLength = 128;

    private static Validation Validate(string value)
        => value.Length > MaxLength
            ? Validation.Invalid($"{nameof(CurrencyName)} must be no more than {MaxLength} characters")
            : Validation.Ok;
}
