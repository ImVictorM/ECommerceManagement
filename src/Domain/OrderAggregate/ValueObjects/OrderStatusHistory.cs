using SharedKernel.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents the historic status change of an order.
/// </summary>
public sealed class OrderStatusHistory : ValueObject
{
    /// <summary>
    /// Gets the order status id.
    /// </summary>
    public long OrderStatusId { get; }

    /// <summary>
    /// Gets the date the order status changed.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    private OrderStatusHistory() { }

    private OrderStatusHistory(long orderStatusId)
    {
        OrderStatusId = orderStatusId;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    /// <param name="orderStatusId">The order status.</param>
    /// <returns>A new instance of the <see cref="OrderStatusHistory"/> class.</returns>
    public static OrderStatusHistory Create(long orderStatusId)
    {
        return new OrderStatusHistory(orderStatusId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return OrderStatusId;
        yield return CreatedAt;
    }
}
