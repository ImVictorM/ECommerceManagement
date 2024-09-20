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
    /// Gets the product discount ids.
    /// </summary>
    public IEnumerable<DiscountId> DiscountIds { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductDiscount"/> class.
    /// </summary>
    /// <param name="discountIds">The product discount ids.</param>
    private ProductDiscount(IEnumerable<DiscountId> discountIds) : base(ProductDiscountId.Create())
    {
        DiscountIds = discountIds;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductDiscount"/> class.
    /// </summary>
    /// <param name="discountIds">The product discount ids.</param>
    /// <returns>a new instance of the <see cref="ProductDiscount"/> class.</returns>
    public static ProductDiscount Create(IEnumerable<DiscountId> discountIds)
    {
        return new ProductDiscount(discountIds);
    }
}
