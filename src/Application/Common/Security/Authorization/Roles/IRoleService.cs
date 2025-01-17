using Application.Common.Security.Identity;

namespace Application.Common.Security.Authorization.Roles;

/// <summary>
/// Represents a contract that defines services related to roles.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Verifies if certain user is an administrator.
    /// </summary>
    /// <param name="user">The current user.</param>
    /// <returns>A boolean value indicating if the user is an administrator.</returns>
    bool IsAdmin(IdentityUser user);

    /// <summary>
    /// Verifies if certain user is an administrator.
    /// </summary>
    /// <param name="id">The current user id.</param>
    /// <returns>A boolean value indicating if the user is an administrator.</returns>
    Task<bool> IsAdminAsync(string id);
}
