using Ardalis.Specification;

namespace IdentityService.Core.UserAggregate.Specifications;

public class UserByNameSpec : Specification<User>, ISingleResultSpecification<User>
{
    public UserByNameSpec(UserName userName)
    {
        Query.Where(user => user.Name == userName);
    }
}
