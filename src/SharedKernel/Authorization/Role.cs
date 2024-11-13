using SharedKernel.Extensions;
using SharedKernel.Models;

namespace SharedKernel.Authorization;

/// <summary>
/// Represents a role.
/// </summary>
public sealed class Role : BaseEnumeration
{
    /// <summary>
    /// Represents the administrator role.
    /// </summary>
    public static readonly Role Admin = new(1, nameof(Admin).ToLowerSnakeCase());

    /// <summary>
    /// Represents the customer role.
    /// </summary>
    public static readonly Role Customer = new(2, nameof(Customer).ToLowerSnakeCase());

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    private Role() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    /// <param name="id">The role identifier.</param>
    private Role(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Gets a role by name, or null if not found.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <returns>The role or null.</returns>
    public static Role GetRoleByName(string name)
    {
        return List().First(r => r.Name == name);
    }

    /// <summary>
    /// Checks if any role name corresponds to the admin role name.
    /// </summary>
    /// <param name="roleNames">The role names to be checked against.</param>
    /// <returns>A bool value indicating if the role names contains the admin role.</returns>
    public static bool HasAdminRole(IEnumerable<string> roleNames)
    {
        return roleNames.Any(name => name == Admin.Name);
    }

    /// <summary>
    /// Checks if any role id corresponds to the admin role id.
    /// </summary>
    /// <param name="roleIds">The role ids to be checked against.</param>
    /// <returns>A bool value indicating if the role ids contains the admin role.</returns>
    public static bool HasAdminRole(IEnumerable<long> roleIds)
    {
        return roleIds.Any(id => id == Admin.Id);
    }

    /// <summary>
    /// Gets all the roles in a list format.
    /// </summary>
    /// <returns>All the roles.</returns>
    public static IEnumerable<Role> List()
    {
        return [Customer, Admin];
    }

    /// <summary>
    /// Gets the roles applying a filter condition.
    /// </summary>
    /// <param name="filter">The filter condition.</param>
    /// <returns>A list with roles that matches the filter condition.</returns>
    public static IEnumerable<Role> List(Func<Role, bool> filter)
    {
        return List().Where(filter);
    }
}
