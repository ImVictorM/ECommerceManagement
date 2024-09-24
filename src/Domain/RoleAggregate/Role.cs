using Domain.Common.Models;
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
    private Role(string name) : base(RoleId.Create())
    {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    public static Role Create(string name)
    {
        return new Role(name);
    }
}
