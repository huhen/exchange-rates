using CurrencyService.Core.CurrencyAggregate.Events;

namespace CurrencyService.Core.CurrencyAggregate;

public sealed class Currency : EntityBase<Currency, CurrencyId>, IAggregateRoot
{
    public CurrencyCharCode CharCode { get; private set; }
    public CurrencyName Name { get; private set; }
    public CurrencyRate Rate { get; private set; }

    public Currency(CurrencyCharCode charCode, CurrencyName name)
    {
        CharCode = charCode;
        Name = name;
    }

    public Currency UpdateName(CurrencyName newName)
    {
        if (Name.Equals(newName)) return this;
        Name = newName;
        return this;
    }

    public Currency UpdateRate(CurrencyRate newRate)
    {
        if (Rate.Equals(newRate)) return this;

        Rate = newRate;
        // RegisterDomainEvent(new CurrencyRateUpdatedEvent(this));
        return this;
    }
}
