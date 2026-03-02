using IdentityService.Core.UserAggregate;
using IdentityService.Core.UserAggregate.Specifications;

namespace IdentityService.UseCases.Users.GetPasswordHash;

public class GetPasswordHashHandler(IReadRepository<User> repository)
    : IQueryHandler<GetPasswordHashQuery, UserIdAndHashDto?>
{
    /// <summary>
    /// Retrieves the user ID and password hash for a user with the specified name.
    /// </summary>
    /// <param name="request">The query containing the user's name to look up.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>`UserIdAndHashDto` with the user's ID and password hash if a matching user exists, `null` otherwise.</returns>
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
