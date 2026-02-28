using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;

namespace IdentityService.Infrastructure.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext dbContext, IServiceProvider services)
    {
        if (await dbContext.Users.AnyAsync()) return; // DB has been seeded

        await PopulateTestDataAsync(dbContext, services);
    }

    private static async Task PopulateTestDataAsync(AppDbContext dbContext, IServiceProvider services)
    {
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();
        
        User user1 = new(UserName.From("admin"), passwordHasher.Hash(UserPassword.From("admin1234")));
        User user2 = new(UserName.From("test"), passwordHasher.Hash(UserPassword.From("test1234")));
        
        dbContext.Users.AddRange([user1, user2]);
        await dbContext.SaveChangesAsync();
    }
}
