using Domain.Common.Models;
using Domain.DiscountAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;

namespace Domain.OrderAggregate.Entities;

/// <summary>
/// Holds the order discount ids.
/// </summary>
public sealed class OrderDiscount : Entity<OrderDiscountId>
{
    /// <summary>
    /// Gets the discount ids.
    /// </summary>
    public DiscountId DiscountId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderDiscount"/> class.
    /// </summary>
    private OrderDiscount() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderDiscount"/> class.
    /// </summary>
    /// <param name="discountId">The related discount ids.</param>
    private OrderDiscount(DiscountId discountId) : base(OrderDiscountId.Create())
    {
        DiscountId = discountId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderDiscount"/> class.
    /// </summary>
    /// <param name="discountId">The related discount ids.</param>
    /// <returns>A new instance of the <see cref="OrderDiscount"/> class.</returns>
    public static OrderDiscount Create(DiscountId discountId)
    {
        return new OrderDiscount(discountId);
    }
}