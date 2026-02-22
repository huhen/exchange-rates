namespace IdentityService.UnitTests.Core.UserAggregate.Events;

// public class UserPasswordUpdatedEventTests
// {
//     [Fact]
//     public void Constructor_SetsUser()
//     {
//         var name = UserName.From("test-user");
//         var password = UserPassword.From("Password1");
//         var user = new User(name, password);
//
//         var domainEvent = new UserPasswordUpdatedEvent(user);
//
//         domainEvent.User.ShouldBe(user);
//     }
//
//     [Fact]
//     public void DateOccurred_IsSetOnCreation()
//     {
//         var name = UserName.From("test-user");
//         var password = UserPassword.From("Password1");
//         var user = new User(name, password);
//
//         var beforeCreate = DateTime.UtcNow;
//         var domainEvent = new UserPasswordUpdatedEvent(user);
//         var afterCreate = DateTime.UtcNow;
//
//         domainEvent.DateOccurred.ShouldNotBe(default);
//         domainEvent.DateOccurred.ShouldBeGreaterThan(beforeCreate);
//         domainEvent.DateOccurred.ShouldBeLessThanOrEqualTo(afterCreate);
//     }
// }
