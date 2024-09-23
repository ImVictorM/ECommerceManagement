using Domain.AddressAggregate.ValueObjects;
using Domain.Common.Models;
using Domain.OrderAggregate.Entities;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.OrderAggregate;

/// <summary>
/// Represents an order.
/// </summary>
public sealed class Order : AggregateRoot<OrderId>
{
    /// <summary>
    /// Gets the order total amount.
    /// </summary>
    public float Total { get; private set; }
    /// <summary>
    /// Gets the order owner id.
    /// </summary>
    public UserId UserId { get; private set; }
    /// <summary>
    /// Gets the order address id.
    /// </summary>
    public AddressId AddressId { get; private set; }
    /// <summary>
    /// Gets the shipment id.
    /// </summary>
    public ShipmentId ShipmentId { get; private set; }
    /// <summary>
    /// Gets the order products.
    /// </summary>
    public IEnumerable<OrderProduct> OrderProducts { get; private set; }
    /// <summary>
    /// Gets the order discounts.
    /// </summary>
    public IEnumerable<OrderDiscount>? OrderDiscounts { get; private set; }
    /// <summary>
    /// Gets the order status.
    /// </summary>
    public OrderStatus OrderStatus { get; private set; }
    /// <summary>
    /// Gets the order status history.
    /// </summary>
    public OrderStatusHistory OrderStatusHistory { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="userId">The order owner id.</param>
    /// <param name="addressId">The order delivery address.</param>
    /// <param name="shipmentId">The order shipment id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="orderStatus">The order status.</param>
    /// <param name="orderStatusHistory">The order status history.</param>
    /// <param name="total">The order total.</param>
    /// <param name="orderDiscounts">The order discounts.</param>
    private Order(
        UserId userId,
        AddressId addressId,
        ShipmentId shipmentId,
        IEnumerable<OrderProduct> orderProducts,
        OrderStatus orderStatus,
        OrderStatusHistory orderStatusHistory,
        float total,
        IEnumerable<OrderDiscount>? orderDiscounts
    ) : base(OrderId.Create())
    {
        UserId = userId;
        AddressId = addressId;
        ShipmentId = shipmentId;
        OrderProducts = orderProducts;
        OrderStatus = orderStatus;
        OrderStatusHistory = orderStatusHistory;
        Total = total;
        OrderDiscounts = orderDiscounts;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="userId">The order owner id.</param>
    /// <param name="addressId">The order delivery address.</param>
    /// <param name="shipmentId">The order shipment id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="orderStatus">The order status.</param>
    /// <param name="orderStatusHistory">The order status history.</param>
    /// <param name="total">The order total.</param>
    /// <param name="orderDiscounts">The order discounts.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public static Order Create(
        UserId userId,
        AddressId addressId,
        ShipmentId shipmentId,
        IEnumerable<OrderProduct> orderProducts,
        OrderStatus orderStatus,
        OrderStatusHistory orderStatusHistory,
        float total,
        IEnumerable<OrderDiscount>? orderDiscounts
    )
    {
        return new Order(
            userId,
            addressId,
            shipmentId,
            orderProducts,
            orderStatus,
            orderStatusHistory,
            total,
            orderDiscounts
        );
    }
}
