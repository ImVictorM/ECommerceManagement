using Domain.Common.Models;
using Domain.OrderStatusAggregate.ValueObjects;

namespace Domain.OrderStatusAggregate;

/// <summary>
/// Represents an order status.
/// </summary>
public sealed class OrderStatus : AggregateRoot<OrderStatusId>
{
    /// <summary>
    /// Gets the order status.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatus"/> class.
    /// </summary>
    private OrderStatus() { }

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
