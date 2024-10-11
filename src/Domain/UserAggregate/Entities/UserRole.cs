using Domain.Common.Models;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate.Entities;

/// <summary>
/// Represent user related roles.
/// </summary>
public sealed class UserRole : Entity<UserRoleId>
{
    /// <summary>
    /// Gets the user role.
    /// </summary>
    public Role Role { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    private UserRole() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="role">The user role.</param>
    private UserRole(
        Role role
    )
    {
        Role = role;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="role">The user role.</param>
    /// <returns>A new instance of the <see cref="UserRole"/> class.</returns>
    public static UserRole Create(Role role)
    {
        return new UserRole(role);
    }
}
