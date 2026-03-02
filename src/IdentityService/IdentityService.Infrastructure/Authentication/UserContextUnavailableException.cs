namespace IdentityService.Infrastructure.Authentication;

public sealed class UserContextUnavailableException() : Exception("User context is unavailable");
