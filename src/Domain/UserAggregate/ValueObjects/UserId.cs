using Domain.Common.Models;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="User"/> aggregate root.
/// </summary>
public sealed class UserId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserId"/> class.
    /// </summary>
    private UserId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private UserId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static UserId Create()
    {
        return new UserId(0);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="UserId"/> class with the specified value.</returns>
    public static UserId Create(long value)
    {
        return new UserId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
