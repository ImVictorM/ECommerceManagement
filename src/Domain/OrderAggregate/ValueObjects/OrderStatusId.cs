using Domain.Common.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// References the identifier for the <see cref="Entities.OrderStatus"/> entity.
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
    /// <param name="value">The identifier value.</param>
    private OrderStatusId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatusId"/> class with default identifier
    /// placeholder value of 0.
    /// </summary>
    /// <returns>A new instance of the <see cref="OrderStatusId"/> class.</returns>
    public static OrderStatusId Create()
    {
        return new OrderStatusId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
