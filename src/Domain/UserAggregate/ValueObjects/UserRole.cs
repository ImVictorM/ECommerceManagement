using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represent a user role.
/// </summary>
public sealed class UserRole : ValueObject
{
    private readonly long _roleId;

    /// <summary>
    /// Gets the role.
    /// </summary>
    public Role Role
    {
        get => BaseEnumeration.FromValue<Role>(_roleId);
    }

    private UserRole() { }

    private UserRole(Role role)
    {
        _roleId = role.Id;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserRole"/> class.
    /// </summary>
    /// <param name="role">The role.</param>
    /// <returns>
    /// A new instance of the <see cref="UserRole"/> class.
    /// </returns>
    public static UserRole Create(Role role)
    {
        return new UserRole(role);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return _roleId;
    }
}
