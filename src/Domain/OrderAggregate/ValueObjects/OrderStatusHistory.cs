using Domain.Common.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Holds an historic change of status of an order.
/// </summary>
public sealed class OrderStatusHistory : ValueObject
{
    /// <summary>
    /// Gets the order status of the change.
    /// </summary>
    public OrderStatus OrderStatus { get; } = null!;

    /// <summary>
    /// Gets the date the order status was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    private OrderStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    /// <param name="orderStatus">The order status.</param>
    private OrderStatusHistory(OrderStatus orderStatus)
    {
        OrderStatus = orderStatus;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    /// <param name="orderStatus">The order status.</param>
    /// <returns>A new instance of the <see cref="OrderStatusHistory"/> class.</returns>
    public static OrderStatusHistory Create(OrderStatus orderStatus)
    {
        return new OrderStatusHistory(orderStatus);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return OrderStatus;
        yield return CreatedAt;
    }
}
