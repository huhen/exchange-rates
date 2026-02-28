using EntityFramework.Exceptions.Common;
using IdentityService.Core.UserAggregate;
using IdentityService.UseCases.Abstractions.Authentication;

namespace IdentityService.UseCases.Users.Register;

public class RegisterUserHandler(
    IRepository<User> repository,
    IPasswordHasher passwordHasher
) : ICommandHandler<RegisterUserCommand, Result<UserId>>
{
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
