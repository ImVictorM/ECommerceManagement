using Domain.Common.Models;
using Domain.Common.ValueObjects;
using Domain.OrderAggregate.Entities;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.OrderAggregate;

/// <summary>
/// Represents an order.
/// </summary>
public sealed class Order : AggregateRoot<OrderId>
{
    /// <summary>
    /// The order products.
    /// </summary>
    private readonly List<OrderProduct> _orderProducts = [];
    /// <summary>
    /// The order discounts.
    /// </summary>
    private readonly List<OrderDiscount>? _orderDiscounts = [];
    /// <summary>
    /// The order status change histories.
    /// </summary>
    private readonly List<OrderStatusHistory> _orderStatusHistories = [];

    /// <summary>
    /// Gets the order total amount.
    /// </summary>
    public float Total { get; private set; }
    /// <summary>
    /// Gets the order owner id.
    /// </summary>
    public UserId UserId { get; private set; } = null!;
    /// <summary>
    /// Gets the order address.
    /// </summary>
    public Address Address { get; private set; } = null!;
    /// <summary>
    /// Gets the order status identifier.
    /// </summary>
    public OrderStatusId OrderStatusId { get; private set; } = null!;
    /// <summary>
    /// Gets the order products.
    /// </summary>
    public IReadOnlyList<OrderProduct> OrderProducts => _orderProducts.AsReadOnly();
    /// <summary>
    /// Gets the order status history.
    /// </summary>
    public IReadOnlyList<OrderStatusHistory> OrderStatusHistories => _orderStatusHistories.AsReadOnly();
    /// <summary>
    /// Gets the order discounts.
    /// </summary>
    public IReadOnlyList<OrderDiscount>? OrderDiscounts => _orderDiscounts?.AsReadOnly();

    /// <summary>
    /// Initiates a new instance of the <see cref="Order"/> class.
    /// </summary>
    private Order() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="userId">The order owner id.</param>
    /// <param name="address">The order delivery address.</param>
    /// <param name="orderStatusId">The order status id.</param>
    /// <param name="total">The order total.</param>
    private Order(
        UserId userId,
        Address address,
        OrderStatusId orderStatusId,
        float total
    )
    {
        UserId = userId;
        Address = address;
        OrderStatusId = orderStatusId;
        Total = total;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="userId">The order owner id.</param>
    /// <param name="address">The order delivery address.</param>
    /// <param name="orderStatusId">The order status id.</param>
    /// <param name="total">The order total.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public static Order Create(
        UserId userId,
        Address address,
        OrderStatusId orderStatusId,
        float total
    )
    {
        // TODO: check if the order status  is paid or pending for new orders.

        return new Order(
            userId,
            address,
            orderStatusId,
            total
        );
    }
}
