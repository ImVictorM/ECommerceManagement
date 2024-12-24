using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.CouponAggregate.ValueObjects;

/// <summary>
/// Represents a coupon order.
/// </summary>
public class CouponOrder : ValueObject
{
    /// <summary>
    /// Gets the order products.
    /// </summary>
    public HashSet<(ProductId ProductId, IEnumerable<CategoryId> ProductCategories)> Products { get; } = null!;
    /// <summary>
    /// Gets the order total.
    /// </summary>
    public decimal Total { get; }

    private CouponOrder() { }

    private CouponOrder(
        HashSet<(ProductId ProductId, IEnumerable<CategoryId> ProductCategories)> products,
        decimal total
    )
    {
        Products = products;
        Total = total;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponOrder"/> class.
    /// </summary>
    /// <param name="products">The order products.</param>
    /// <param name="total">The order total.</param>
    /// <returns>A new instance of the <see cref="CouponOrder"/> class.</returns>
    public static CouponOrder Create(
        HashSet<(ProductId ProductId, IEnumerable<CategoryId> ProductCategories)> products,
        decimal total
    )
    {
        return new CouponOrder(products, total);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Products;
        yield return Total;
    }
}
