using Domain.Common.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// References the identifier for the <see cref="Entities.OrderStatusHistory"/> entity.
/// </summary>
public sealed class OrderStatusHistoryId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of <see cref="OrderStatusHistoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private OrderStatusHistoryId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderStatusHistoryId"/> class with default identifier placeholder of 0.
    /// </summary>
    /// <returns>A new instance of <see cref="OrderStatusHistoryId"/> class.</returns>
    public static OrderStatusHistoryId Create()
    {
        return new OrderStatusHistoryId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
