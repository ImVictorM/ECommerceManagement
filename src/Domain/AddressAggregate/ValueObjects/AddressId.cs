using Domain.Common.Models;

namespace Domain.AddressAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Address"/> entity.
/// </summary>
public sealed class AddressId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="AddressId"/> class.
    /// </summary>
    private AddressId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="AddressId"/> class.
    /// </summary>
    /// <param name="value">The <see cref="AddressId"/> identifier.</param>
    private AddressId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AddressId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="AddressId"/> class with the specified value.</returns>
    public static AddressId Create(long value)
    {
        return new AddressId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
