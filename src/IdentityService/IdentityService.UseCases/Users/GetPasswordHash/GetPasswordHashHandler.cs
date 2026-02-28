using IdentityService.Core.UserAggregate;
using IdentityService.Core.UserAggregate.Specifications;

namespace IdentityService.UseCases.Users.GetPasswordHash;

public class GetPasswordHashHandler(IReadRepository<User> repository)
    : IQueryHandler<GetPasswordHashQuery, UserIdAndHashDto?>
{
    public async ValueTask<UserIdAndHashDto?> Handle(GetPasswordHashQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new UserByNameSpec(request.Name);
        var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        return entity is null
            ? null
            : new UserIdAndHashDto(entity.Id, entity.PasswordHash);
    }
}
