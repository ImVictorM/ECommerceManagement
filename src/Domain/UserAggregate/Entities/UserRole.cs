using Domain.Common.Models;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate.Entities;

/// <summary>
/// Represent user related roles.
/// </summary>
public sealed class UserRole : Entity<UserRoleId>
{
    /// <summary>
    /// Gets the role identifier.
    /// </summary>
    public RoleId RoleId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    private UserRole() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="roleId">The user role.</param>
    private UserRole(
        RoleId roleId
    )
    {
        RoleId = roleId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="roleId">The user role id.</param>
    /// <returns>A new instance of the <see cref="UserRole"/> class.</returns>
    public static UserRole Create(RoleId roleId)
    {
        return new UserRole(roleId);
    }
}
