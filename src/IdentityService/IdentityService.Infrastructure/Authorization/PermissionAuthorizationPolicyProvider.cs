using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    private readonly AuthorizationOptions _authorizationOptions = options.Value;

    /// <summary>
    /// Resolves an authorization policy by name, creating and registering a permission-based policy if none exists.
    /// </summary>
    /// <param name="policyName">The name of the policy to resolve; used as the permission identifier when creating a new policy.</param>
    /// <returns>
    /// The existing <see cref="AuthorizationPolicy"/> with the specified name if found; otherwise a newly created <see cref="AuthorizationPolicy"/> that contains a <see cref="PermissionRequirement"/> for the given name.
    /// </returns>
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
        {
            return policy;
        }

        AuthorizationPolicy permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();

        _authorizationOptions.AddPolicy(policyName, permissionPolicy);

        return permissionPolicy;
    }
}
