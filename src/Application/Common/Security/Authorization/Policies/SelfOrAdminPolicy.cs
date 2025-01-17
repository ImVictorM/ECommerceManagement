using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Defines a policy that authorizes the user if the current user is the same
/// as the request user or if the current user is an administrator.
/// </summary>
public sealed class SelfOrAdminPolicy : IPolicy
{
    private readonly IRoleService _roleService;

    internal SelfOrAdminPolicy(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <inheritdoc/>
    public Task<bool> IsAuthorizedAsync<T>(RequestWithAuthorization<T> request, IdentityUser currentUser)
    {
        if (request.UserId == null)
        {
            throw new ArgumentException($"The user id is required for the authorization process. Policy: {nameof(SelfOrAdminPolicy)}");
        }

        return Task.FromResult(request.UserId == currentUser.Id || _roleService.IsAdmin(currentUser));
    }
}
