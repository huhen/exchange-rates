using IdentityService.Core.UserAggregate;

namespace IdentityService.UseCases.Users;

public record UserIdAndHashDto(UserId Id, UserPasswordHash PasswordHash);
