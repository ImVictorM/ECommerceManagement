using Domain.Common.Models;
using Domain.DiscountAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.ProductAggregate.Entities;

/// <summary>
/// Represents a product discount.
/// </summary>
public sealed class ProductDiscount : Entity<ProductDiscountId>
{
    /// <summary>
    /// Gets the product discount id.
    /// </summary>
    public DiscountId DiscountId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductDiscount"/> class.
    /// </summary>
    private ProductDiscount() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductDiscount"/> class.
    /// </summary>
    /// <param name="discountId">The product discount id.</param>
    private ProductDiscount(DiscountId discountId) : base(ProductDiscountId.Create())
    {
        DiscountId = discountId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductDiscount"/> class.
    /// </summary>
    /// <param name="discountId">The product discount id.</param>
    /// <returns>a new instance of the <see cref="ProductDiscount"/> class.</returns>
    public static ProductDiscount Create(DiscountId discountId)
    {
        return new ProductDiscount(discountId);
    }
}
