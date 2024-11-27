using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate;

/// <summary>
/// Represents an order.
/// </summary>
public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderProduct> _products = [];
    private readonly List<OrderStatusHistory> _orderStatusHistories = [];
    private readonly List<Discount> _discounts = [];

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
    public long OrderStatusId { get; private set; }
    /// <summary>
    /// Gets the order products.
    /// </summary>
    public IReadOnlyList<OrderProduct> Products => _products.AsReadOnly();
    /// <summary>
    /// Gets the order status history.
    /// </summary>
    public IReadOnlyList<OrderStatusHistory> OrderStatusHistories => _orderStatusHistories.AsReadOnly();
    /// <summary>
    /// Gets the order discounts.
    /// </summary>
    public IReadOnlyList<Discount> Discounts => _discounts.AsReadOnly();

    private Order() { }

    private Order(
        UserId userId,
        Address address,
        OrderStatus orderStatus,
        float total
    )
    {
        UserId = userId;
        Address = address;
        OrderStatusId = orderStatus.Id;
        Total = total;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="userId">The order owner id.</param>
    /// <param name="address">The order delivery address.</param>
    /// <param name="total">The order total.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public static Order Create(
        UserId userId,
        Address address,
        float total
    )
    {
        var order = new Order(
            userId,
            address,
            OrderStatus.Pending,
            total
        );

        order.AddDomainEvent(new OrderCreated(order));

        return order;
    }
}
