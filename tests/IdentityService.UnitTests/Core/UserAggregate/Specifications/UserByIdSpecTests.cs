namespace IdentityService.UnitTests.Core.UserAggregate.Specifications;

public class UserByIdSpecTests
{
    [Fact]
    public void Constructor_SetsUserIdFilter()
    {
        var userId = UserId.From(Guid.NewGuid());
        var spec = new UserByIdSpec(userId);

        var user1 = new User(UserName.From("user.one"), UserPassword.From("Password1"));
        var user2 = new User(UserName.From("user.two"), UserPassword.From("Password2"));
        user1.Id = userId;
        user2.Id = UserId.From(Guid.NewGuid());

        var result = spec.Evaluate(new[] { user1, user2 }).ToList();

        result.ShouldHaveSingleItem();
        result.Single().Id.ShouldBe(userId);
    }

    [Fact]
    public void Evaluate_WhenNoMatchingUser_ReturnsEmpty()
    {
        var userId = UserId.From(Guid.NewGuid());
        var spec = new UserByIdSpec(userId);

        var user1 = new User(UserName.From("user.one"), UserPassword.From("Password1"));
        var user2 = new User(UserName.From("user.two"), UserPassword.From("Password2"));

        var result = spec.Evaluate(new[] { user1, user2 }).ToList();

        result.ShouldBeEmpty();
    }

    [Fact]
    public void Evaluate_WhenMultipleUsersWithSameId_ReturnsAllMatching()
    {
        var userId = UserId.From(Guid.NewGuid());
        var spec = new UserByIdSpec(userId);

        var user1 = new User(UserName.From("user.one"), UserPassword.From("Password1"));
        var user2 = new User(UserName.From("user.two"), UserPassword.From("Password2"));
        user1.Id = userId;
        user2.Id = userId;

        var result = spec.Evaluate(new[] { user1, user2 }).ToList();

        result.Count.ShouldBe(2);
        result.All(u => u.Id == userId).ShouldBeTrue();
    }
}
