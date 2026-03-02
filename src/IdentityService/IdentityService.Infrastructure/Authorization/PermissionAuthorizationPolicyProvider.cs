using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    private readonly ConcurrentDictionary<string, AuthorizationPolicy> _policyCache = new();

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

        return _policyCache.GetOrAdd(policyName, name =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(name))
                .Build());
    }
}
