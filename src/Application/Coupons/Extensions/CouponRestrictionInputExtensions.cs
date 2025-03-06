using Application.Coupons.Abstracts;
using Application.Coupons.DTOs;
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
public static class CouponRestrictionInputExtensions
{
    /// <summary>
    /// Creates a domain <see cref="CouponRestriction"/> from the provided
    /// <see cref="ICouponRestrictionInput"/>.
    /// </summary>
    /// <param name="input">
    /// The coupon restriction input data transfer object.
    /// </param>
    /// <returns>
    /// A domain coupon restriction constructed from the input.
    /// </returns>
    /// <exception cref="NotSupportedRestrictionTypeException">
    /// Thrown when the input does not match any supported coupon restriction type.
    /// </exception>
    public static CouponRestriction ParseRestriction(
        this ICouponRestrictionInput input
    )
    {
        return input switch
        {
            CategoryRestrictionInput categoryInput => CategoryRestriction.Create(
                categoryInput.CategoryAllowedIds
                    .Select(CategoryId.Create)
                    .Select(CouponCategory.Create),
                categoryInput.ProductFromCategoryNotAllowedIds?
                    .Select(ProductId.Create)
                    .Select(CouponProduct.Create)
            ),
            ProductRestrictionInput productInput => ProductRestriction.Create(
                productInput.ProductAllowedIds
                    .Select(ProductId.Create)
                    .Select(CouponProduct.Create)
            ),
            _ => throw new NotSupportedRestrictionTypeException()
        };
    }

    /// <summary>
    /// Converts a collection of <see cref="ICouponRestrictionInput"/> objects
    /// into a collection of <see cref="CouponRestriction"/> objects.
    /// </summary>
    /// <param name="inputs">A collection of coupon restriction input.</param>
    /// <returns>
    /// A collection of domain <see cref="CouponRestriction"/> objects constructed
    /// from the input. If the input collection is null, an empty collection
    /// is returned.
    /// </returns>
    public static IEnumerable<CouponRestriction> ParseRestrictions(
        this IEnumerable<ICouponRestrictionInput>? inputs
    )
    {
        if (inputs == null)
        {
            return [];
        }

        return inputs.Select(ParseRestriction);
    }
}
