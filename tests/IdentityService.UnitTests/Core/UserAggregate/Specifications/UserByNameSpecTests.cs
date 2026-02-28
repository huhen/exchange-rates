using IdentityService.Core.UserAggregate;
using IdentityService.Core.UserAggregate.Specifications;

namespace IdentityService.UnitTests.Core.UserAggregate.Specifications;

public class UserByNameSpecTests
{
    [Fact]
    public void Query_WhenUserWithMatchingName_ReturnsUser()
    {
        var userName = UserName.From("test-user");
        var spec = new UserByNameSpec(userName);
        var user = new User(userName, UserPasswordHash.From("Password1"));

        var result = spec.Evaluate([user]).SingleOrDefault();

        result.ShouldBe(user);
    }

    [Fact]
    public void Query_WhenNoMatchingUser_ReturnsNull()
    {
        var userName = UserName.From("test-user");
        var spec = new UserByNameSpec(userName);
        var otherUser = new User(UserName.From("other-user"), UserPasswordHash.From("Password1"));

        var result = spec.Evaluate([otherUser]).SingleOrDefault();

        result.ShouldBeNull();
    }

    [Fact]
    public void Query_WhenMultipleUsersWithMatchingName_ReturnsOnlyMatching()
    {
        var userName = UserName.From("test-user");
        var spec = new UserByNameSpec(userName);
        var user1 = new User(userName, UserPasswordHash.From("Password1"));
        var user2 = new User(UserName.From("other-user"), UserPasswordHash.From("Password1"));

        var result = spec.Evaluate([user1, user2]).SingleOrDefault();

        result.ShouldBe(user1);
    }
}
