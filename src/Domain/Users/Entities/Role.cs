using Domain.Common.Models;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities;

/// <summary>
/// Represents an user role.
/// </summary>
public sealed class Role : Entity<RoleId>
{
    /// <summary>
    /// Gets the role name.
    /// </summary>
    public string Name { get; }

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
