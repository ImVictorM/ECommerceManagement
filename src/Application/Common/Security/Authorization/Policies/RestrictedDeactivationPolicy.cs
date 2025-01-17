using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Represents a policy to deactivate an user.
/// A non-admin user can deactivate themselves.
/// An admin can deactivate other non-admin users.
/// </summary>
public sealed class RestrictedDeactivationPolicy : IPolicy
{
    private readonly IRoleService _roleService;

    internal RestrictedDeactivationPolicy(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <inheritdoc/>
    public async Task<bool> IsAuthorizedAsync<T>(RequestWithAuthorization<T> request, IdentityUser currentUser)
    {
        if (request.UserId == null)
        {
            throw new ArgumentException($"The user id is required for the authorization process. Policy: {nameof(RestrictedDeactivationPolicy)}");
        }

        var userToBeDeactivatedId = request.UserId;
        var currentUserIsAdmin = _roleService.IsAdmin(currentUser);

        if (currentUser.Id != userToBeDeactivatedId)
        {
            var userToBeDeactivatesIsAdmin = await _roleService.IsAdminAsync(userToBeDeactivatedId);

            // Only admins are allowed to deactivate other users that are not admins
            return currentUserIsAdmin && !userToBeDeactivatesIsAdmin;
        }
        else
        {
            // Admins are not allowed to deactivate themselves
            return !currentUserIsAdmin;
        }
    }
}
