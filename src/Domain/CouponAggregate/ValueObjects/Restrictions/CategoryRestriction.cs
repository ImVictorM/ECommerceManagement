using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate.Abstracts;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Errors;

namespace Domain.CouponAggregate.ValueObjects.Restrictions;

/// <summary>
/// Represents a coupon restriction defined by categories allowed and products excluded.
/// </summary>
public class CategoryRestriction : CouponRestriction
{
    /// <summary>
    /// Gets the categories the restriction allows.
    /// </summary>
    public IEnumerable<CouponCategory> CategoriesAllowed { get; } = null!;
    /// <summary>
    /// Gets the products that are not allowed.
    /// </summary>
    public IEnumerable<CouponProduct> ProductsFromCategoryNotAllowed { get; } = null!;

    private CategoryRestriction() { }

    private CategoryRestriction(
        IEnumerable<CouponCategory> categoriesAllowed,
        IEnumerable<CouponProduct>? productsFromNotAllowed = null
    )
    {
        CategoriesAllowed = categoriesAllowed;
        ProductsFromCategoryNotAllowed = productsFromNotAllowed ?? [];
    }

    internal static CategoryRestriction Create(
        IEnumerable<CouponCategory> categoriesAllowed,
        IEnumerable<CouponProduct>? productsFromCategoryNotAllowed = null
    )
    {
        if (!categoriesAllowed.Any())
        {
            throw new DomainValidationException($"Restriction must contain at least one category");
        }

        return new CategoryRestriction(categoriesAllowed, productsFromCategoryNotAllowed);
    }

    /// <inheritdoc/>
    public override bool PassRestriction(CouponOrder order)
    {
        return order.Products.Any(IsProductAllowed);
    }

    private bool IsProductAllowed((ProductId ProductId, IReadOnlySet<CategoryId> ProductCategories) product)
    {
        return IsProductNotExcluded(product.ProductId) && HasAnyAllowedCategory(product.ProductCategories);
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

    ///<inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CategoriesAllowed;
        yield return ProductsFromCategoryNotAllowed;
    }
}
