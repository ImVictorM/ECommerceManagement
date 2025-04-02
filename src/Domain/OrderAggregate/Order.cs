using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Errors;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
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
    private long _orderStatusId;
    private readonly HashSet<OrderLineItem> _products = [];
    private readonly HashSet<OrderTrackingEntry> _orderTrackingEntries = [];
    private readonly HashSet<OrderCoupon> _couponsApplied = [];

    /// <summary>
    /// Gets the order total.
    /// </summary>
    public decimal Total { get; private set; }
    /// <summary>
    /// Gets the order owner identifier.
    /// </summary>
    public UserId OwnerId { get; private set; } = null!;
    /// <summary>
    /// Gets the order description.
    /// </summary>
    public string Description { get; private set; } = null!;
    /// <summary>
    /// Gets the order status.
    /// </summary>
    public OrderStatus OrderStatus
    {
        get => BaseEnumeration.FromValue<OrderStatus>(_orderStatusId);
        private set => _orderStatusId = value.Id;
    }
    /// <summary>
    /// Gets the order products.
    /// </summary>
    public IReadOnlySet<OrderLineItem> Products => _products;
    /// <summary>
    /// Gets the order tracking entries.
    /// </summary>
    public IReadOnlySet<OrderTrackingEntry> OrderTrackingEntries => _orderTrackingEntries;
    /// <summary>
    /// Gets the order coupons applied.
    /// </summary>
    public IReadOnlySet<OrderCoupon> CouponsApplied => _couponsApplied;

    private Order() { }

    private Order(
        UserId ownerId,
        IEnumerable<OrderLineItem> products,
        decimal total,
        IEnumerable<OrderCoupon>? couponsApplied
    )
    {
        OwnerId = ownerId;
        Total = total;
        Description = "Order pending. Waiting for payment";

        _products.UnionWith(products);

        var initialOrderStatus = OrderStatus.Pending;

        _orderStatusId = initialOrderStatus.Id;
        _orderTrackingEntries.Add(OrderTrackingEntry.Create(initialOrderStatus));

        if (couponsApplied != null)
        {
            if (couponsApplied.Count() > 2)
            {
                throw new OutOfRangeException(
                    "An order can contain a maximum of two applied coupons"
                );
            }

            _couponsApplied.UnionWith(couponsApplied);
        }
    }

    internal static Order Create(
        Guid requestId,
        UserId userId,
        ShippingMethodId shippingMethodId,
        IEnumerable<OrderLineItem> products,
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
            total,
            couponsApplied
        );

        order.AddDomainEvent(new OrderCreated(
            requestId,
            shippingMethodId,
            order,
            paymentMethod,
            deliveryAddress,
            billingAddress,
            installments
        ));

        return order;
    }

    /// <summary>
    /// Cancels the order by setting its status to <see cref="OrderStatus.Canceled"/>.
    /// </summary>
    /// <param name="reason">The reason for cancellation.</param>
    /// <exception cref="InvalidOrderCancellationException">
    /// Thrown if the order is not in a pending state.
    /// </exception>
    public void Cancel(string reason)
    {
        if (OrderStatus != OrderStatus.Pending)
        {
            throw new InvalidOrderCancellationException();
        }

        UpdateOrderStatus(OrderStatus.Canceled, reason);
        AddDomainEvent(new OrderCanceled(this));
    }

    /// <summary>
    /// Marks the order as paid by updating its status to
    /// <see cref="OrderStatus.Paid"/>.
    /// </summary>
    /// <exception cref="InvalidOrderStateForPaymentException">
    /// Thrown if the order is not in a pending state.
    /// </exception>
    public void MarkAsPaid()
    {
        if (OrderStatus != OrderStatus.Pending)
        {
            throw new InvalidOrderStateForPaymentException();
        }

        UpdateOrderStatus(OrderStatus.Paid, "The order was paid successfully");
        AddDomainEvent(new OrderPaid(this));
    }

    private void UpdateOrderStatus(OrderStatus status, string description)
    {
        _orderStatusId = status.Id;
        Description = description;
        _orderTrackingEntries.Add(OrderTrackingEntry.Create(status));
    }
}
