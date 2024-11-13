using Domain.OrderAggregate.ValueObjects;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate.Entities;

/// <summary>
/// Holds the order discount ids.
/// </summary>
public sealed class OrderDiscount : Entity<OrderDiscountId>
{
    /// <summary>
    /// Gets the discount.
    /// </summary>
    public Discount Discount { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderDiscount"/> class.
    /// </summary>
    private OrderDiscount() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderDiscount"/> class.
    /// </summary>
    /// <param name="discount">The related discount.</param>
    private OrderDiscount(Discount discount)
    {
        Discount = discount;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderDiscount"/> class.
    /// </summary>
    /// <param name="discount">The related discount ids.</param>
    /// <returns>A new instance of the <see cref="OrderDiscount"/> class.</returns>
    public static OrderDiscount Create(Discount discount)
    {
        return new OrderDiscount(discount);
    }
}
