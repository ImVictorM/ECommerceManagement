using SharedKernel.Models;

namespace Application.Common.Security.Authorization.Roles;

/// <summary>
/// Represents a role.
/// </summary>
public sealed class Role : BaseEnumeration
{
    /// <summary>
    /// Represents the administrator role.
    /// </summary>
    public static readonly Role Admin = new(1, nameof(Admin));

    /// <summary>
    /// Represents the customer role.
    /// </summary>
    public static readonly Role Customer = new(2, nameof(Customer));

    private Role() { }

    private Role(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Retrieves a role by the role name.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <returns>The role.</returns>
    public static Role FromDisplayName(string name)
    {
        return FromDisplayName<Role>(name);
    }

    /// <summary>
    /// Retrieves a role by the role identifier.
    /// </summary>
    /// <param name="value">The role identifier.</param>
    /// <returns>The role.</returns>
    public static Role FromValue(long value)
    {
        return FromValue<Role>(value);
    }

    /// <summary>
    /// Verifies if a list of roles names contains the admin role name.
    /// </summary>
    /// <param name="roleNames">The role names.</param>
    /// <returns>A bool value indicating if the roles contain the admin role name.</returns>
    public static bool HasAdminRole(IEnumerable<string> roleNames)
    {
        return roleNames.Contains(Admin.Name);
    }

    /// <summary>
    /// Verifies if a list of roles ids contains the admin role id.
    /// </summary>
    /// <param name="roleIds">The role ids.</param>
    /// <returns>A bool value indicating if the role ids contain the admin role id.</returns>
    public static bool HasAdminRole(IEnumerable<long> roleIds)
    {
        return roleIds.Contains(Admin.Id);
    }

    /// <summary>
    /// Gets all the roles in a list format.
    /// </summary>
    /// <returns>All the roles.</returns>
    public static IReadOnlyList<Role> List()
    {
        return GetAll<Role>().ToList();
    }
}
