using Domain.Common.Models;
using Domain.OrderAggregate.ValueObjects;

namespace Domain.OrderAggregate.Entities;

/// <summary>
/// Holds an historic change of status of an order.
/// </summary>
public sealed class OrderStatusHistory : Entity<OrderStatusHistoryId>
{
    /// <summary>
    /// Gets the order status change history.
    /// </summary>
    public IEnumerable<OrderStatusId> OrderStatuses { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    private OrderStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    /// <param name="orderStatuses">The order statuses history.</param>
    private OrderStatusHistory(IEnumerable<OrderStatusId> orderStatuses) : base(OrderStatusHistoryId.Create())
    {
        OrderStatuses = orderStatuses;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatusHistory"/> class.
    /// </summary>
    /// <param name="orderStatuses">The order statuses history.</param>
    /// <returns>A new instance of the <see cref="OrderStatusHistory"/> class.</returns>
    public static OrderStatusHistory Create(IEnumerable<OrderStatusId> orderStatuses)
    {
        return new OrderStatusHistory(orderStatuses);
    }
}
