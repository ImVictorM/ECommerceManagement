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
    /// Gets all the roles in a list format.
    /// </summary>
    /// <returns>All the roles.</returns>
    public static IReadOnlyList<Role> List()
    {
        return GetAll<Role>().ToList();
    }
}
