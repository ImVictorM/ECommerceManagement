namespace Application.Common.Security.Authorization.Roles;

/// <summary>
/// Utilities for the <see cref="Role"/> class.
/// </summary>
public static class RoleUtils
{
    private static readonly IReadOnlyList<Role> _availableRoles = Role.List();
    private static readonly Dictionary<long, Role> _availableRolesById = _availableRoles.ToDictionary(r => r.Id);
    private static readonly Dictionary<string, Role> _availableRolesByName = _availableRoles.ToDictionary(r => r.Name);

    /// <summary>
    /// Retrieves a role by its identifier.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <returns>The role.</returns>
    public static Role FromValue(long roleId)
    {
        return _availableRolesById[roleId];
    }

    /// <summary>
    /// Retrieves a role by its name.
    /// </summary>
    /// <param name="roleName">The role name.</param>
    /// <returns>The role.</returns>
    public static Role FromDisplayName(string roleName)
    {
        return _availableRolesByName[roleName];
    }
}
