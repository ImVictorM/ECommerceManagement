using Domain.Common.Models;
using Domain.UserAggregate.Entities;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="UserRole"/> aggregate root.
/// </summary>
public sealed class UserRoleId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    private UserRoleId() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoleId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private UserRoleId(long value)
    {
        Value = value;
    }

    // <summary>
    /// Creates a new instance of the <see cref="UserRoleId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static UserRoleId Create()
    {
        return new UserRoleId(0);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserRoleId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="UserRoleId"/> class with the specified identifier value.</returns>
    public static UserRoleId Create(long value)
    {
        return new UserRoleId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
