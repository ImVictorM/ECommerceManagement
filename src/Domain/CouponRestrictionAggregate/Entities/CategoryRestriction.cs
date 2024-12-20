using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponRestrictionAggregate.DTOs;
using Domain.CouponRestrictionAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Errors;

namespace Domain.CouponRestrictionAggregate.Entities;

/// <summary>
/// Represents a coupon restriction defined by categories allowed and products excluded.
/// </summary>
public class CategoryRestriction : CouponRestriction
{
    /// <summary>
    /// Gets the categories the restriction allows.
    /// </summary>
    public IEnumerable<CategoryRestricted> CategoriesAllowed { get; } = null!;
    /// <summary>
    /// Gets the products that are not allowed.
    /// </summary>
    public IEnumerable<ProductRestricted> ProductsFromCategoryNotAllowed { get; } = null!;

    private CategoryRestriction() { }

    private CategoryRestriction(
        CouponId couponId,
        IEnumerable<CategoryRestricted> categoriesAllowed,
        IEnumerable<ProductRestricted>? productsFromNotAllowed = null
    ) : base(couponId)
    {
        CategoriesAllowed = categoriesAllowed;
        ProductsFromCategoryNotAllowed = productsFromNotAllowed ?? [];
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CategoryRestriction"/> class.
    /// </summary>
    /// <param name="couponId">The coupon id.</param>
    /// <param name="categoriesAllowed">The categories allowed.</param>
    /// <param name="productsFromCategoryNotAllowed">The products not allowed.</param>
    /// <returns>A new instance of the <see cref="CategoryRestriction"/> class.</returns>
    /// <exception cref="DomainValidationException">Thrown when the categories allowed is empty.</exception>
    public static CategoryRestriction Create(
        CouponId couponId,
        IEnumerable<CategoryRestricted> categoriesAllowed,
        IEnumerable<ProductRestricted>? productsFromCategoryNotAllowed = null
    )
    {
        if (!categoriesAllowed.Any())
        {
            throw new DomainValidationException($"Restriction must contain at least one category");
        }

        return new CategoryRestriction(couponId, categoriesAllowed, productsFromCategoryNotAllowed);
    }

    /// <inheritdoc/>
    public override bool PassRestriction(CouponRestrictionContext context)
    {
        return context.Products.Any(IsProductAllowed);
    }

    private bool IsProductAllowed(ContextProduct product)
    {
        return IsProductNotExcluded(product.ProductId) && HasAnyAllowedCategory(product.CategoryIds);
    }

    private bool HasAnyAllowedCategory(IEnumerable<CategoryId> categories)
    {
        var categoryAllowedIds = CategoriesAllowed.Select(ca => ca.CategoryId);
        return categories.Intersect(categoryAllowedIds).Any();
    }

    private bool IsProductNotExcluded(ProductId productId)
    {
        var prohibitedProductIds = ProductsFromCategoryNotAllowed.Select(p => p.ProductId);

        return !prohibitedProductIds.Contains(productId);
    }
}
