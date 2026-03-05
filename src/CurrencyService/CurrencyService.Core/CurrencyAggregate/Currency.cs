using CurrencyService.Core.CurrencyAggregate.Events;

namespace CurrencyService.Core.CurrencyAggregate;

public sealed class Currency : EntityBase<Currency, CurrencyId>, IAggregateRoot
{
    public CurrencyCharCode CharCode { get; private set; }
    public CurrencyName Name { get; private set; }
    public CurrencyRate Rate { get; private set; }

    /// <summary>
    /// Initializes a new Currency with the specified character code and display name.
    /// </summary>
    /// <param name="charCode">The currency's character code.</param>
    /// <param name="name">The currency's display name.</param>
    /// <param name="rate">The currency's exchange rate</param>
    public Currency(CurrencyCharCode charCode, CurrencyName name, CurrencyRate rate)
    {
        CharCode = charCode;
        Name = name;
        Rate = rate;
    }

    /// <summary>
    /// Update the currency's display name when it differs from the current value.
    /// </summary>
    /// <param name="newName">The new currency name to apply.</param>
    /// <returns>The current <see cref="Currency"/> instance; modified if the name was changed.</returns>
    public Currency UpdateName(CurrencyName newName)
    {
        if (Name.Equals(newName)) return this;
        Name = newName;
        return this;
    }

    /// <summary>
    /// Updates the currency's exchange rate if it differs from the current rate.
    /// </summary>
    /// <param name="newRate">The new exchange rate to apply.</param>
    /// <returns>The current <see cref="Currency"/> instance after applying the update.</returns>
    public Currency UpdateRate(CurrencyRate newRate)
    {
        if (Rate.Equals(newRate)) return this;

        Rate = newRate;
        // RegisterDomainEvent(new CurrencyRateUpdatedEvent(this));
        return this;
    }
}
