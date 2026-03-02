namespace ExchangeRates.SharedKernel;

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Removes all domain events currently held by the implementing object.
    /// </summary>
    /// <remarks>
    /// After invocation, the <see cref="DomainEvents"/> collection is expected to be empty.
    /// </remarks>
    void ClearDomainEvents();
}
