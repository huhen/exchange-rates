using EntityFramework.Exceptions.Common;
using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;

namespace IdentityService.UseCases.Users.Register;

public class RegisterUserHandler(
    IRepository<User> repository,
    IPasswordHasher passwordHasher
) : ICommandHandler<RegisterUserCommand, Result<UserId>>
{
    /// <summary>
    /// Processes a registration command by creating a new user, persisting it, and returning the created user's identifier.
    /// </summary>
    /// <param name="command">Registration data containing the user's name and password.</param>
    /// <returns>`Result<UserId>` containing the created user's Id on success; a failure result with message "User already exists" if a user with the same unique constraint already exists.</returns>
    public async ValueTask<Result<UserId>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var newUser = new User(command.Name, passwordHasher.Hash(command.Password));

        try
        {
            var createdItem = await repository.AddAsync(newUser, cancellationToken);
            return Result.Success(createdItem.Id);
        }
        catch (UniqueConstraintException)
        {
            return Result.Failure<UserId>("User already exists");
        }
    }
}
