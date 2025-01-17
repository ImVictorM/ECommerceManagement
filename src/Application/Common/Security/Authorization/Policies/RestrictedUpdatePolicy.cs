using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Represents a policy to update a user.
/// Users can update themselves. Admins can update other non-admin users.
/// </summary>
public sealed class RestrictedUpdatePolicy : IPolicy
{
    private readonly IRoleService _roleService;

    internal RestrictedUpdatePolicy(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <inheritdoc/>
    public async Task<bool> IsAuthorizedAsync<T>(RequestWithAuthorization<T> request, IdentityUser currentUser)
    {
        if (request.UserId == null)
        {
            throw new ArgumentException($"The user id is required for the authorization process. Policy: {nameof(RestrictedUpdatePolicy)}");
        }

        var userToBeUpdatedId = request.UserId;
        var currentUserIsAdmin = _roleService.IsAdmin(currentUser);
        var userToBeUpdatedIsAdmin = await _roleService.IsAdminAsync(userToBeUpdatedId);

        return currentUser.Id == userToBeUpdatedId || (currentUserIsAdmin && !userToBeUpdatedIsAdmin);
    }
}
