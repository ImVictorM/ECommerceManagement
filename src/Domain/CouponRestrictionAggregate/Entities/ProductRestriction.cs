using Domain.CouponAggregate.ValueObjects;
using Domain.CouponRestrictionAggregate.DTOs;
using Domain.CouponRestrictionAggregate.ValueObjects;
using SharedKernel.Errors;

namespace Domain.CouponRestrictionAggregate.Entities;

/// <summary>
/// Represents a coupon restriction that defines some products to be allowed.
/// </summary>
public class ProductRestriction : CouponRestriction
{
    /// <summary>
    /// Gets the products allowed.
    /// </summary>
    public IEnumerable<ProductRestricted> ProductsAllowed { get; } = null!;

    private ProductRestriction() { }

    private ProductRestriction(CouponId couponId, IEnumerable<ProductRestricted> productsAllowed) : base(couponId)
    {
        ProductsAllowed = productsAllowed;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductRestriction"/> class.
    /// </summary>
    /// <param name="couponId">The coupon id.</param>
    /// <param name="productsAllowed">The products allowed.</param>
    /// <returns>A new instance of the <see cref="ProductRestriction"/> class.</returns>
    /// <exception cref="DomainValidationException">Thrown when the products allowed is empty.</exception>
    public static ProductRestriction Create(CouponId couponId, IEnumerable<ProductRestricted> productsAllowed)
    {
        if (!productsAllowed.Any())
        {
            throw new DomainValidationException($"Restriction must contain at least one product");
        }

        return new ProductRestriction(couponId, productsAllowed);
    }

    /// <inheritdoc/>
    public override bool PassRestriction(CouponRestrictionContext context)
    {
        var productsAllowedIds = ProductsAllowed.Select(pa => pa.ProductId);
        var productIds = context.Products.Select(pa => pa.ProductId);

        return productsAllowedIds.Intersect(productIds).Any();
    }
}
