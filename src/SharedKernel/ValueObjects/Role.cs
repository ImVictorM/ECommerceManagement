using SharedKernel.Models;

namespace SharedKernel.ValueObjects;

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

    /// <summary>
    /// Represents the carrier role.
    /// </summary>
    public static readonly Role Carrier = new(3, nameof(Carrier));

    private Role() { }

    private Role(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Lists all of the defined roles.
    /// </summary>
    /// <returns>
    /// A list of containing all the defined <see cref="Role"/>.
    /// </returns>
    public static IReadOnlyList<Role> List()
    {
        return GetAll<Role>().ToList();
    }
}
