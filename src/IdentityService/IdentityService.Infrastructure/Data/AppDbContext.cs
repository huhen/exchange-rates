using System.Reflection;
using IdentityService.Core.UserAggregate;
using IdentityService.Infrastructure.Data.Config;

namespace IdentityService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="AppDbContext"/> with the specified options.
    /// </summary>
    /// <param name="options">The options used to configure the database context.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Configures the Entity Framework Core model for this context by setting the default database schema
    /// and applying entity configurations from the executing assembly.
    /// </summary>
    /// <param name="modelBuilder">The builder used to configure entity types, relationships, and mappings.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schemas.Default);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Registers EF Core conventions that add value object converters for all Vogen types.
    /// </summary>
    /// <param name="configurationBuilder">The builder used to configure model-wide conventions.</param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.RegisterAllInVogenEfCoreConverters();
    }

    /// <summary>
    /// Persists changes in the context to the database and returns the number of affected entries.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}
