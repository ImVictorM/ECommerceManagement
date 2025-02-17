using Domain.CouponAggregate.Abstracts;
using SharedKernel.Errors;

namespace Domain.CouponAggregate.ValueObjects.Restrictions;

/// <summary>
/// Represents a coupon restriction that defines some products to be allowed.
/// </summary>
public class ProductRestriction : CouponRestriction
{
    /// <summary>
    /// Gets the products allowed.
    /// </summary>
    public IEnumerable<CouponProduct> ProductsAllowed { get; } = null!;

    private ProductRestriction() { }

    private ProductRestriction(IEnumerable<CouponProduct> productsAllowed)
    {
        ProductsAllowed = productsAllowed;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductRestriction"/> class.
    /// </summary>
    /// <param name="productsAllowed">The products allowed.</param>
    /// <returns>A new instance of the <see cref="ProductRestriction"/> class.</returns>
    /// <exception cref="EmptyArgumentException">Thrown when the products allowed list is empty.</exception>
    public static ProductRestriction Create(IEnumerable<CouponProduct> productsAllowed)
    {
        if (!productsAllowed.Any())
        {
            throw new EmptyArgumentException("Restriction must contain at least one product");
        }

        return new ProductRestriction(productsAllowed);
    }

    /// <inheritdoc/>
    public override bool PassRestriction(CouponOrder order)
    {
        var productsAllowedIds = ProductsAllowed.Select(pa => pa.ProductId);
        var productIds = order.Products.Select(pa => pa.ProductId);

        return productsAllowedIds.Intersect(productIds).Any();
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductsAllowed;
    }
}
