using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users.Register;

public class RegisterUserHandler(IRepository<User> repository)
    : ICommandHandler<RegisterUserCommand, Result<UserId>>
{
    public async ValueTask<Result<UserId>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var newUser = new User(command.Name, command.Password);

        var createdItem = await repository.AddAsync(newUser, cancellationToken);

        return Result.Success(createdItem.Id);
    }
}
