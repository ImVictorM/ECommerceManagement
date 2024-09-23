using Domain.Common.Models;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Entities.UserAddress"/> entity.
/// </summary>
public sealed class UserAddressId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    private UserAddressId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAddressId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private UserAddressId(long value)
    {
        Value = value;
    }

    // <summary>
    /// Creates a new instance of the <see cref="UserAddressId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static UserAddressId Create()
    {
        return new UserAddressId(0);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserAddressId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="UserAddressId"/> class with the specified identifier.</returns>
    public static UserAddressId Create(long value)
    {
        return new UserAddressId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
