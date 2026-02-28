namespace ExchangeRates.SharedKernel;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot;
