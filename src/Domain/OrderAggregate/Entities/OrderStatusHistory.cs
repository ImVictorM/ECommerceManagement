using Domain.Common.Models;
using Domain.OrderAggregate.ValueObjects;
using Domain.OrderStatusAggregate.ValueObjects;

namespace Domain.OrderAggregate.Entities;

/// <summary>
/// Holds an historic change of status of an order.
/// </summary>
public sealed class OrderStatusHistory : Entity<OrderStatusHistoryId>
{
    /// <summary>
    /// Gets the order status id of the change.
    /// </summary>
    public OrderStatusId OrderStatusId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    private OrderStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    /// <param name="orderStatusId">The order statuses history.</param>
    private OrderStatusHistory(OrderStatusId orderStatusId)
    {
        OrderStatusId = orderStatusId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    /// <param name="orderStatusId">The order statuses history.</param>
    /// <returns>A new instance of the <see cref="OrderStatusHistory"/> class.</returns>
    public static OrderStatusHistory Create(OrderStatusId orderStatusId)
    {
        return new OrderStatusHistory(orderStatusId);
    }
}
