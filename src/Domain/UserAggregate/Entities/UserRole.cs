using Domain.Common.Models;
using Domain.RoleAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate.Entities;

/// <summary>
/// Represent user related roles.
/// </summary>
public sealed class UserRole : Entity<UserRoleId>
{
    /// <summary>
    /// Gets the user role ids.
    /// </summary>
    public RoleId RoleId { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="roleId">The user role id.</param>
    private UserRole(
        RoleId roleId
    ) : base(UserRoleId.Create())
    {
        RoleId = roleId;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private UserRole() : base(UserRoleId.Create()) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


    /// <summary>
    /// Creates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="roleId">The user roles.</param>
    /// <returns>A new instance of the <see cref="UserRole"/> class.</returns>
    public static UserRole Create(RoleId roleId)
    {
        return new UserRole(roleId);
    }
}
