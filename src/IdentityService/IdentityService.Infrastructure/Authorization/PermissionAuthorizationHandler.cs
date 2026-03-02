using IdentityService.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// Evaluates the permission requirement against the current user and marks the requirement succeeded if the user is authenticated and belongs to the role named by the requirement.
    /// </summary>
    /// <param name="context">The authorization context containing the current user and evaluation methods.</param>
    /// <param name="requirement">The permission requirement whose Permission value is used as the required role name.</param>
    /// <returns>A completed Task; the requirement is marked succeeded when the user is authenticated and is in the required role.</returns>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User is { Identity.IsAuthenticated: true })
        {
            if (context.User.IsInRole(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
