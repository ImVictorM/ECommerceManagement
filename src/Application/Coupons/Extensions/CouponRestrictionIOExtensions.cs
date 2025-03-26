using Application.Coupons.DTOs.Restrictions;
using Application.Coupons.Errors;

using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate.Abstracts;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate.ValueObjects;

namespace Application.Coupons.Extensions;

/// <summary>
/// Provides extension methods to convert coupon restriction input DTOs
/// into domain coupon restriction objects.
/// </summary>
public static class CouponRestrictionIOExtensions
{
    /// <summary>
    /// Converts a domain <see cref="CouponRestriction"/> into a
    /// <see cref="CouponRestrictionIO"/>.
    /// </summary>
    /// <param name="restriction">The domain coupon restriction.</param>
    /// <returns>
    /// A <see cref="CouponRestrictionIO"/> representing the domain restriction.
    /// </returns>
    /// <exception cref="NotSupportedRestrictionTypeException">
    /// Thrown when the restriction type is not supported.
    /// </exception>
    public static CouponRestrictionIO ParseRestriction(
        this CouponRestriction restriction
    )
    {
        return restriction switch
        {
            CouponProductRestriction pr =>
                new CouponProductRestrictionIO(
                    pr.ProductsAllowed.Select(p => p.ProductId.ToString())
                ),

            CouponCategoryRestriction cr =>
                new CouponCategoryRestrictionIO(
                    cr.CategoriesAllowed.Select(c => c.CategoryId.ToString()),
                    cr.ProductsFromCategoryNotAllowed.Select(p => p.ProductId.ToString())
                ),

            _ => throw new NotSupportedRestrictionTypeException()
        };
    }

    /// <summary>
    /// Converts a domain <see cref="CouponRestrictionIO"/> into
    /// a <see cref="CouponRestriction"/>.
    /// </summary>
    /// <param name="io">
    /// The coupon restriction input data transfer object.
    /// </param>
    /// <returns>
    /// A domain coupon restriction constructed from the input.
    /// </returns>
    /// <exception cref="NotSupportedRestrictionTypeException">
    /// Thrown when the input does not match any supported coupon restriction type.
    /// </exception>
    public static CouponRestriction ParseRestriction(
        this CouponRestrictionIO io
    )
    {
        return io switch
        {
            CouponCategoryRestrictionIO categoryInput => CouponCategoryRestriction.Create(
                categoryInput.CategoryAllowedIds
                    .Select(CategoryId.Create)
                    .Select(CouponCategory.Create),
                categoryInput.ProductFromCategoryNotAllowedIds?
                    .Select(ProductId.Create)
                    .Select(CouponProduct.Create)
            ),
            CouponProductRestrictionIO productInput => CouponProductRestriction.Create(
                productInput.ProductAllowedIds
                    .Select(ProductId.Create)
                    .Select(CouponProduct.Create)
            ),
            _ => throw new NotSupportedRestrictionTypeException()
        };
    }

    /// <summary>
    /// Converts a collection of <see cref="CouponRestrictionIO"/> objects
    /// into a collection of <see cref="CouponRestriction"/> objects.
    /// </summary>
    /// <param name="inputs">A collection of coupon restriction input.</param>
    /// <returns>
    /// A collection of domain <see cref="CouponRestriction"/> objects constructed
    /// from the input. If the input collection is null, an empty collection
    /// is returned.
    /// </returns>
    public static IEnumerable<CouponRestriction> ParseRestrictions(
        this IEnumerable<CouponRestrictionIO>? inputs
    )
    {
        if (inputs == null)
        {
            return [];
        }

        return inputs.Select(ParseRestriction);
    }

    /// <summary>
    /// Converts a collection of <see cref="CouponRestriction"/> objects
    /// into a collection of <see cref="CouponRestrictionIO"/> objects.
    /// </summary>
    /// <param name="restrictions">The domain coupon restrictions.</param>
    /// <returns>A collection of <see cref="CouponRestrictionIO"/> objects.</returns>
    public static IEnumerable<CouponRestrictionIO> ParseRestrictions(
        this IEnumerable<CouponRestriction> restrictions
    )
    {
        return restrictions.Select(ParseRestriction);
    }

}
