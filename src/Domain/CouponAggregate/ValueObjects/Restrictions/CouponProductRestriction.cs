using Domain.CouponAggregate.Abstracts;

using SharedKernel.Errors;

namespace Domain.CouponAggregate.ValueObjects.Restrictions;

/// <summary>
/// Represents a coupon restriction that defines some products to be allowed.
/// </summary>
public class CouponProductRestriction : CouponRestriction
{
    private readonly List<CouponProduct> _productsAllowed = [];

    /// <summary>
    /// Gets the products allowed.
    /// </summary>
    public IReadOnlyList<CouponProduct> ProductsAllowed => _productsAllowed;

    private CouponProductRestriction() { }

    private CouponProductRestriction(IEnumerable<CouponProduct> productsAllowed)
    {
        _productsAllowed = productsAllowed.ToList();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponProductRestriction"/> class.
    /// </summary>
    /// <param name="productsAllowed">The products allowed.</param>
    /// <returns>
    /// A new instance of the <see cref="CouponProductRestriction"/> class.
    /// </returns>
    /// <exception cref="EmptyArgumentException">
    /// Thrown when the products allowed list is empty.
    /// </exception>
    public static CouponProductRestriction Create(
        IEnumerable<CouponProduct> productsAllowed
    )
    {
        if (!productsAllowed.Any())
        {
            throw new EmptyArgumentException(
                "Restriction must contain at least one product"
            );
        }

        return new CouponProductRestriction(productsAllowed);
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
        foreach (var productAllowed in ProductsAllowed)
        {
            yield return productAllowed;
        }
    }
}
