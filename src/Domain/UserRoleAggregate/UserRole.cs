using Domain.Common.Models;
using Domain.UserAggregate.ValueObjects;
using Domain.UserRoleAggregate.Entities;
using Domain.UserRoleAggregate.ValueObjects;

namespace Domain.UserRoleAggregate;

/// <summary>
/// Represent user related roles.
/// </summary>
public sealed class UserRole : AggregateRoot<UserRoleId>
{
    /// <summary>
    /// Gets the user id.
    /// </summary>
    public UserId UserId { get; private set; }
    /// <summary>
    /// Gets the user roles.
    /// </summary>
    public IEnumerable<Role> Roles { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="roles">The user roles.</param>
    private UserRole(
        UserId userId,
        IEnumerable<Role> roles
    ) : base(UserRoleId.Create())
    {
        UserId = userId;
        Roles = roles;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="roles">The user roles.</param>
    /// <returns>A new instance of the <see cref="UserRole"/> class.</returns>
    public static UserRole Create(UserId userId, IEnumerable<Role> roles)
    {
        return new UserRole(userId, roles);
    }
}
