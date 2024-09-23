using Domain.RoleAggregate;

namespace Application.Common.Interfaces.Persistence;

/// <summary>
/// Repository to interact and persist user roles.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Adds a single role to the given user.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="roleName">The role name.</param>
    /// <returns>The user roles.</returns>
    Task<IReadOnlyList<Role>> AssignRoleToUserAsync(long userId, string roleName);

    /// <summary>
    /// Adds multiple roles to the given user.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="roleNames">The roles to be added.</param>
    /// <returns></returns>
    Task<IReadOnlyList<Role>> AssignRoleToUserAsync(long userId, string[] roleNames);

    Task<IReadOnlyList<Role>> GetUserRolesAsync(long userId);
}
