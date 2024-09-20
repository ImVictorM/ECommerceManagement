using Domain.Common.Models;
using Domain.OrderAggregate.ValueObjects;

namespace Domain.OrderAggregate.Entities;

/// <summary>
/// Holds the status of an <see cref="Order"/>
/// </summary>
public sealed class OrderStatus : Entity<OrderStatusId>
{
    /// <summary>
    /// Gets the order status.
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatus"/> class.
    /// </summary>
    /// <param name="status">The order status.</param>
    private OrderStatus(string status) : base(OrderStatusId.Create())
    {
        Status = status;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatus"/> class.
    /// </summary>
    /// <param name="status">The order status.</param>
    /// <returns>A new instance of the <see cref="OrderStatus"/> class.</returns>
    public static OrderStatus Create(string status)
    {
        return new OrderStatus(status);
    }
}
