using Ardalis.Specification;

namespace IdentityService.Core.UserAggregate.Specifications;

public class UserByIdSpec : Specification<User>, ISingleResultSpecification<User>
{
    public UserByIdSpec(UserId userId)
    {
        Query.Where(user => user.Id == userId);
    }
}
