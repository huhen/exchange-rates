using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;

namespace IdentityService.Infrastructure.Data;

public static class SeedData
{
    /// <summary>
    /// Seeds initial user data into the database if the Users table is empty.
    /// </summary>
    /// <param name="dbContext">The application's database context used to query and persist user entities.</param>
    /// <param name="services">The service provider used to resolve dependencies required for seeding (for example, a password hasher).</param>
    public static async Task InitializeAsync(AppDbContext dbContext, IServiceProvider services)
    {
        if (await dbContext.Users.AnyAsync()) return; // DB has been seeded

        await PopulateTestDataAsync(dbContext, services);
    }

    /// <summary>
    /// Creates two test user records ("admin" and "test") with hashed passwords and saves them to the provided database context.
    /// </summary>
    /// <param name="dbContext">The application's database context used to add and persist user entities.</param>
    /// <param name="services">The service provider used to resolve required services (for example, the password hasher).</param>
    private static async Task PopulateTestDataAsync(AppDbContext dbContext, IServiceProvider services)
    {
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();

        User user1 = new(UserName.From("admin"), passwordHasher.Hash(UserPassword.From("admin1234")));
        User user2 = new(UserName.From("test"), passwordHasher.Hash(UserPassword.From("test1234")));

        dbContext.Users.AddRange([user1, user2]);
        await dbContext.SaveChangesAsync();
    }
}
