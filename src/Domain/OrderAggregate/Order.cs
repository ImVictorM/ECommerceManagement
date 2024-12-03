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
public sealed class Order : AggregateRoot<OrderId>, IDiscountable
{
    private readonly List<OrderProduct> _products = [];
    private readonly List<OrderStatusHistory> _orderStatusHistories = [];
    private readonly List<Discount> _discounts = [];

    /// <summary>
    /// Gets the order total amount without discounts applied.
    /// </summary>
    public decimal BaseTotal { get; private set; }
    /// <summary>
    /// Gets the order owner id.
    /// </summary>
    public UserId UserId { get; private set; } = null!;
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
        IEnumerable<OrderProduct> products,
        OrderStatus orderStatus,
        decimal baseTotal
    )
    {
        UserId = userId;
        OrderStatusId = orderStatus.Id;
        BaseTotal = baseTotal;

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
    /// <param name="installments">The installments.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public static Order Create(
        UserId userId,
        IEnumerable<OrderProduct> products,
        decimal baseTotal,
        IPaymentMethod paymentMethod,
        int? installments = null
    )
    {
        var order = new Order(
            userId,
            products,
            OrderStatus.Pending,
            baseTotal
        );

        order.AddDomainEvent(new OrderCreated(order, paymentMethod, installments));

        return order;
    }

    /// <inheritdoc/>
    public void AddDiscounts(params Discount[] discounts)
    {
        _discounts.AddRange(discounts);
    }

    /// <inheritdoc/>
    public void ClearDiscounts()
    {
        _discounts.Clear();
    }

    /// <inheritdoc/>
    public decimal GetPriceAfterDiscounts()
    {
        return DiscountService.ApplyDiscounts(BaseTotal, [.. _discounts]);
    }
}
