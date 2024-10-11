using Domain.Common.Models;
using Domain.Common.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.ProductAggregate.Entities;

/// <summary>
/// Represents a product discount.
/// </summary>
public sealed class ProductDiscount : Entity<ProductDiscountId>
{
    /// <summary>
    /// Gets the product discount.
    /// </summary>
    public Discount Discount { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductDiscount"/> class.
    /// </summary>
    private ProductDiscount() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductDiscount"/> class.
    /// </summary>
    /// <param name="discount">The product discount.</param>
    private ProductDiscount(Discount discount)
    {
        Discount = discount;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductDiscount"/> class.
    /// </summary>
    /// <param name="discount">The product discount.</param>
    /// <returns>a new instance of the <see cref="ProductDiscount"/> class.</returns>
    public static ProductDiscount Create(Discount discount)
    {
        return new ProductDiscount(discount);
    }
}
