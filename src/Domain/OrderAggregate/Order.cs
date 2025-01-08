using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Errors;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate;

/// <summary>
/// Represents an order.
/// </summary>
public sealed class Order : AggregateRoot<OrderId>
{
    private readonly HashSet<OrderProduct> _products = [];
    private readonly HashSet<OrderStatusHistory> _orderStatusHistories = [];
    private readonly HashSet<OrderCoupon> _couponsApplied = [];

    /// <summary>
    /// Gets the order total.
    /// </summary>
    public decimal Total { get; private set; }
    /// <summary>
    /// Gets the order owner id.
    /// </summary>
    public UserId OwnerId { get; private set; } = null!;
    /// <summary>
    /// Gets the order description.
    /// </summary>
    public string Description { get; private set; } = null!;
    /// <summary>
    /// Gets the order status identifier.
    /// </summary>
    public long OrderStatusId { get; private set; }
    /// <summary>
    /// Gets the order products.
    /// </summary>
    public IReadOnlySet<OrderProduct> Products => _products;
    /// <summary>
    /// Gets the order status history.
    /// </summary>
    public IReadOnlySet<OrderStatusHistory> OrderStatusHistories => _orderStatusHistories;
    /// <summary>
    /// Gets the order coupons applied.
    /// </summary>
    public IReadOnlySet<OrderCoupon> CouponsApplied => _couponsApplied;

    private Order() { }

    private Order(
        UserId userId,
        IEnumerable<OrderProduct> products,
        OrderStatus orderStatus,
        decimal total,
        IEnumerable<OrderCoupon>? couponsApplied
    )
    {
        OwnerId = userId;
        OrderStatusId = orderStatus.Id;
        Total = total;
        Description = "Order pending. Waiting for payment";

        _products.UnionWith(products);

        _orderStatusHistories.Add(OrderStatusHistory.Create(orderStatus.Id));

        if (couponsApplied != null)
        {
            if (couponsApplied.Count() > 2)
            {
                throw new OutOfRangeException("An order can contain a maximum of two applied coupons");
            }

            _couponsApplied.UnionWith(couponsApplied);
        }
    }

    internal static Order Create(
        Guid requestId,
        UserId userId,
        IEnumerable<OrderProduct> products,
        decimal total,
        IPaymentMethod paymentMethod,
        Address billingAddress,
        Address deliveryAddress,
        int? installments = null,
        IEnumerable<OrderCoupon>? couponsApplied = null
    )
    {
        var order = new Order(
            userId,
            products,
            OrderStatus.Pending,
            total,
            couponsApplied
        );

        order.AddDomainEvent(
            new OrderCreated(
                requestId,
                order,
                paymentMethod,
                billingAddress,
                deliveryAddress,
                installments
            )
        );

        return order;
    }

    /// <summary>
    /// Cancels an order by setting its status to <see cref="OrderStatus.Canceled"/>.
    /// </summary>
    public void Cancel(string reason)
    {
        UpdateOrderStatus(OrderStatus.Canceled, reason);
        AddDomainEvent(new OrderCanceled(this));
    }

    /// <summary>
    /// Marks the order as paid.
    /// </summary>
    /// <param name="deliveryAddress">The delivery address.</param>
    public void MarkAsPaid(Address deliveryAddress)
    {
        UpdateOrderStatus(OrderStatus.Paid, "The order was paid successfully");
        AddDomainEvent(new OrderPaid(this, deliveryAddress));
    }

    /// <summary>
    /// Retrieves the order status description.
    /// </summary>
    /// <returns>The description of the order status.</returns>
    public string GetStatusDescription()
    {
        var status = BaseEnumeration.FromValue<OrderStatus>(OrderStatusId);

        return status.Name;
    }

    private void UpdateOrderStatus(OrderStatus status, string description)
    {
        OrderStatusId = status.Id;
        Description = description;
        _orderStatusHistories.Add(OrderStatusHistory.Create(status.Id));
    }
}
