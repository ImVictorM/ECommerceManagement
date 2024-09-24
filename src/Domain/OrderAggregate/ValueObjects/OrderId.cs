using Domain.Common.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Order"/> aggregate.
/// </summary>
public sealed class OrderId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    private OrderId() { }

    /// <summary>
    /// Initiates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private OrderId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderId"/> class with default identifier placeholder of 0.
    /// </summary>
    /// <returns>A new instance of <see cref="OrderId"/> class.</returns>
    public static OrderId Create()
    {
        return new OrderId(0);
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="OrderId"/> class with the specified identifier.</returns>
    public static OrderId Create(long value)
    {
        return new OrderId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
