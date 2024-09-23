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
    public IEnumerable<DiscountId> DiscountIds { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderDiscount"/> class.
    /// </summary>
    /// <param name="discountIds">The related discount ids.</param>
    private OrderDiscount(IEnumerable<DiscountId> discountIds) : base(OrderDiscountId.Create())
    {
        DiscountIds = discountIds;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderDiscount"/> class.
    /// </summary>
    /// <param name="discountIds">The related discount ids.</param>
    /// <returns>A new instance of the <see cref="OrderDiscount"/> class.</returns>
    public static OrderDiscount Create(IEnumerable<DiscountId> discountIds)
    {
        return new OrderDiscount(discountIds);
    }
}
