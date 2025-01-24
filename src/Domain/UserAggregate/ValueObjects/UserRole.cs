using SharedKernel.Models;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represent user related roles.
/// </summary>
public sealed class UserRole : ValueObject
{
    /// <summary>
    /// Gets the role identifier.
    /// </summary>
    public long RoleId { get; }

    private UserRole() { }

    private UserRole(long roleId)
    {
        RoleId = roleId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="roleId">The user role id.</param>
    /// <returns>A new instance of the <see cref="UserRole"/> class.</returns>
    public static UserRole Create(long roleId)
    {
        return new UserRole(roleId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return RoleId;
    }
}
