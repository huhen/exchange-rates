namespace ExchangeRates.SharedKernel;

public interface IDomainEvent : INotification
{
    DateTime DateOccurred { get; }
}
