using Ardalis.Specification.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Creates a new EfRepository instance using the provided database context.
    /// </summary>
    /// <param name="dbContext">The application's Entity Framework database context used by the repository.</param>
    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
