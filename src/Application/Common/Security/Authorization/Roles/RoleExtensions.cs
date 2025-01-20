namespace Application.Common.Security.Authorization.Roles;

/// <summary>
/// Extension methods for the <see cref="Role"/> class.
/// </summary>
public static class RoleExtensions
{
    /// <summary>
    /// Verifies if a list of roles contains the admin role.
    /// </summary>
    /// <param name="roles">The roles.</param>
    /// <returns>A bool value indicating if the roles contain the admin role.</returns>
    public static bool HasAdminRole(this IEnumerable<Role> roles)
    {
        return roles.Contains(Role.Admin);
    }
}
