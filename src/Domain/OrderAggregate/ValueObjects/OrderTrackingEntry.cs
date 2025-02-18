using Domain.OrderAggregate.Enumerations;

using SharedKernel.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents an order tracking entry.
/// </summary>
public sealed class OrderTrackingEntry : ValueObject
{
    private readonly long _orderStatusId;

    /// <summary>
    /// Gets the order status.
    /// </summary>
    public OrderStatus OrderStatus
    {
        get => BaseEnumeration.FromValue<OrderStatus>(_orderStatusId);
    }

    /// <summary>
    /// Gets the date the order tracking entry was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    private OrderTrackingEntry() { }

    private OrderTrackingEntry(OrderStatus orderStatus)
    {
        _orderStatusId = orderStatus.Id;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderTrackingEntry"/> class.
    /// </summary>
    /// <param name="orderStatus">The order status.</param>
    /// <returns>A new instance of the <see cref="OrderTrackingEntry"/> class.</returns>
    public static OrderTrackingEntry Create(OrderStatus orderStatus)
    {
        return new OrderTrackingEntry(orderStatus);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return _orderStatusId;
        yield return CreatedAt;
    }
}
