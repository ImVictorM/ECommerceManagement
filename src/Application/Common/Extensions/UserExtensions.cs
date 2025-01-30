using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Application.Common.Security.Authorization.Roles;

namespace Application.Common.Extensions;

/// <summary>
/// Extensions for the <see cref="UserRole"/> class.
/// </summary>
public static class UserExtensions
{
    /// <summary>
    /// Verifies if the user is an administrator.
    /// </summary>
    /// <param name="user">The current user.</param>
    /// <returns>A boolean value indicating if the current user is an administrator.</returns>
    public static bool IsAdmin(this User user)
    {
        var roles = user.UserRoles.Select(ur => RoleUtils.FromValue(ur.RoleId));

        return roles.HasAdminRole();
    }

    /// <summary>
    /// Retrieve the role names for a <see cref="IEnumerable{UserRole}"/> enumerable.
    /// </summary>
    /// <param name="roles">The current user roles.</param>
    /// <returns>A list of the user role names.</returns>
    public static IReadOnlyList<string> GetRoleNames(this IEnumerable<UserRole> roles)
    {
        return roles.Select(r => RoleUtils.FromValue(r.RoleId).Name).ToList();
    }
}
