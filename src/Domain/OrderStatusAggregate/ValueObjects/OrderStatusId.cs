using Domain.Common.Models;

namespace Domain.OrderStatusAggregate.ValueObjects;

/// <summary>
/// References the identifier for the <see cref="OrderStatus"/> entity.
/// </summary>
public sealed class OrderStatusId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatusId"/> class.
    /// </summary>
    private OrderStatusId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatusId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private OrderStatusId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatusId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="OrderStatusId"/> class with the specified identifier.</returns>
    public static OrderStatusId Create(long value)
    {
        return new OrderStatusId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
