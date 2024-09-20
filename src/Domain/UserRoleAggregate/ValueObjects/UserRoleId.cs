using Domain.Common.Models;

namespace Domain.UserRoleAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="UserRole"/> aggregate root.
/// </summary>
public sealed class UserRoleId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

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

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
