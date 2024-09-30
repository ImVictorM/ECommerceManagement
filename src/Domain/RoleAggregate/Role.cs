using Domain.Common.Models;
using Domain.RoleAggregate.Enums;
using Domain.RoleAggregate.ValueObjects;

namespace Domain.RoleAggregate;

/// <summary>
/// Represents a role.
/// </summary>
public sealed class Role : AggregateRoot<RoleId>
{
    /// <summary>
    /// Gets the role name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    private Role() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    private Role(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="roleType">The name of the role.</param>
    public static Role Create(RoleTypes roleType)
    {
        return new Role(ToName(roleType));
    }

    /// <summary>
    /// Maps the role type to correspondent name.
    /// </summary>
    /// <param name="roleType">The role type.</param>
    /// <returns>The correspondent role name.</returns>
    /// <exception cref="ArgumentOutOfRangeException">When the role type is invalid.</exception>
    public static string ToName(RoleTypes roleType)
    {
        return roleType switch
        {
            RoleTypes.CUSTOMER => "customer",
            RoleTypes.ADMIN => "admin",
            _ => throw new ArgumentOutOfRangeException(nameof(roleType)),
        };
    }
}
