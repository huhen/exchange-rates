namespace IdentityService.Core.UserAggregate.Specifications;

public class UserByNameSpec : Specification<User>, ISingleResultSpecification<User>
{
    /// <summary>
    /// Initializes a specification that selects the User whose Name equals the provided UserName.
    /// </summary>
    /// <param name="userName">The user name to match.</param>
    public UserByNameSpec(UserName userName)
    {
        Query.Where(user => user.Name == userName);
    }
}
