using Domain.CouponAggregate.ValueObjects;
using Domain.CouponRestrictionAggregate.Entities;
using Domain.CouponRestrictionAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.CouponRestrictionAggregate.Factories;

/// <summary>
/// Factory to create restrictions.
/// </summary>
public static class CouponRestrictionFactory
{
    /// <summary>
    /// Creates a new instance of the <see cref="CouponRestriction"/> class.
    /// </summary>
    /// <param name="couponId">The coupon id.</param>
    /// <param name="idProductsAllowed">The id of the products allowed.</param>
    /// <returns>A new instance of the <see cref="CouponRestriction"/> class.</returns>
    public static CouponRestriction CreateRestriction(CouponId couponId, IEnumerable<ProductId> idProductsAllowed)
    {
        var productsRestricted = idProductsAllowed.Select(ProductRestricted.Create);

        return ProductRestriction.Create(couponId, productsRestricted);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponRestriction"/> class.
    /// </summary>
    /// <param name="couponId">The coupon id.</param>
    /// <param name="idCategoriesAllowed">The id of the categories allowed.</param>
    /// <param name="idProductsFromCategoryNotAllowed">The product ids to not be allowed.</param>
    /// <returns>A new instance of the <see cref="CouponRestriction"/> class.</returns>
    public static CouponRestriction CreateRestriction(
        CouponId couponId,
        IEnumerable<long> idCategoriesAllowed,
        IEnumerable<ProductId>? idProductsFromCategoryNotAllowed = null
    )
    {
        var productsRestricted = idProductsFromCategoryNotAllowed?.Select(ProductRestricted.Create);
        var categoriesRestricted = idCategoriesAllowed.Select(CategoryRestricted.Create);

        return CategoryRestriction.Create(couponId, categoriesRestricted, productsRestricted);
    }
}
