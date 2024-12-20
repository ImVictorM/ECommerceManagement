using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.Services;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate;

/// <summary>
/// Represents an order.
/// </summary>
public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderProduct> _products = [];
    private readonly List<OrderStatusHistory> _orderStatusHistories = [];
    private readonly List<CouponId> _couponAppliedIds = [];

    /// <summary>
    /// Gets the order total amount without discounts applied.
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
    public IReadOnlyList<OrderProduct> Products => _products.AsReadOnly();
    /// <summary>
    /// Gets the order status history.
    /// </summary>
    public IReadOnlyList<OrderStatusHistory> OrderStatusHistories => _orderStatusHistories.AsReadOnly();
    /// <summary>
    /// Gets the order discounts.
    /// </summary>
    public IReadOnlyList<CouponId> CouponAppliedIds => _couponAppliedIds.AsReadOnly();

    private Order() { }

    private Order(
        UserId userId,
        IEnumerable<OrderProduct> products,
        OrderStatus orderStatus,
        decimal baseTotal
    )
    {
        OwnerId = userId;
        OrderStatusId = orderStatus.Id;
        Total = baseTotal;
        Description = "Order pending. Waiting for payment";

        _products.AddRange(products);
        _orderStatusHistories.Add(OrderStatusHistory.Create(orderStatus.Id));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="userId">The order owner id.</param>
    /// <param name="products">The order products.</param>
    /// <param name="baseTotal">The order total without discounts applied.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="billingAddress">The order payment billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The installments.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public static Order Create(
        UserId userId,
        IEnumerable<OrderProduct> products,
        decimal baseTotal,
        IPaymentMethod paymentMethod,
        Address billingAddress,
        Address deliveryAddress,
        int? installments = null
    )
    {
        var order = new Order(
            userId,
            products,
            OrderStatus.Pending,
            baseTotal
        );

        order.AddDomainEvent(
            new OrderCreated(
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
    public void CancelOrder(string reason)
    {
        UpdateOrderStatus(OrderStatus.Canceled, reason);
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

    /// <inheritdoc/>
    //public decimal CalculateTotalApplyingDiscounts()
    //{
    //    var couponDiscounts = _couponAppliedIds.Select(c => c.Discount);

    //    return DiscountService.ApplyDiscounts(Total, [.. couponDiscounts]);
    //}

    private void UpdateOrderStatus(OrderStatus status, string description)
    {
        OrderStatusId = status.Id;
        Description = description;
        _orderStatusHistories.Add(OrderStatusHistory.Create(status.Id));
    }
}
