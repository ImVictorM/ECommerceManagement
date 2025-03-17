using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate.Abstracts;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Errors;

namespace Domain.CouponAggregate.ValueObjects.Restrictions;

/// <summary>
/// Represents a coupon restriction defined by categories allowed and products
/// excluded.
/// </summary>
public class CouponCategoryRestriction : CouponRestriction
{
    private readonly List<CouponCategory> _categoriesAllowed = [];
    private readonly List<CouponProduct> _productsFromCategoryNotAllowed = [];

    /// <summary>
    /// Gets the categories the restriction allows.
    /// </summary>
    public IReadOnlyList<CouponCategory> CategoriesAllowed => _categoriesAllowed;

    /// <summary>
    /// Gets the products that are not allowed.
    /// </summary>
    public IReadOnlyList<CouponProduct> ProductsFromCategoryNotAllowed
        => _productsFromCategoryNotAllowed;

    private CouponCategoryRestriction() { }

    private CouponCategoryRestriction(
        IEnumerable<CouponCategory> categoriesAllowed,
        IEnumerable<CouponProduct>? productsFromNotAllowed = null
    )
    {
        _categoriesAllowed = categoriesAllowed.ToList();
        _productsFromCategoryNotAllowed = productsFromNotAllowed?.ToList() ?? [];
    }

    /// <summary>
    /// /Creates a new instance of the <see cref="CouponCategoryRestriction"/> class.
    /// </summary>
    /// <param name="categoriesAllowed">
    /// The categories allowed.
    /// </param>
    /// <param name="productsFromCategoryNotAllowed">
    /// The products from the category not allowed.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="CouponCategoryRestriction"/> class.
    /// </returns>
    /// <exception cref="EmptyArgumentException">
    /// Thrown when categories allowed list is empty.
    /// </exception>
    public static CouponCategoryRestriction Create(
        IEnumerable<CouponCategory> categoriesAllowed,
        IEnumerable<CouponProduct>? productsFromCategoryNotAllowed = null
    )
    {
        if (!categoriesAllowed.Any())
        {
            throw new EmptyArgumentException(
                "Restriction must contain at least one category"
            );
        }

        return new CouponCategoryRestriction(
            categoriesAllowed,
            productsFromCategoryNotAllowed
        );
    }

    /// <inheritdoc/>
    public override bool PassRestriction(CouponOrder order)
    {
        return order.Products.Any(IsProductAllowed);
    }

    private bool IsProductAllowed(CouponOrderProduct product)
    {
        return IsProductNotExcluded(product.ProductId)
            && HasAnyAllowedCategory(product.ProductCategoryIds);
    }

    private bool HasAnyAllowedCategory(IEnumerable<CategoryId> categories)
    {
        var categoryAllowedIds = CategoriesAllowed.Select(ca => ca.CategoryId);

        return categories.Intersect(categoryAllowedIds).Any();
    }

    private bool IsProductNotExcluded(ProductId productId)
    {
        var prohibitedProductIds = ProductsFromCategoryNotAllowed
            .Select(p => p.ProductId);

        return !prohibitedProductIds.Contains(productId);
    }

    ///<inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        foreach (var categoryAllowed in CategoriesAllowed)
        {
            yield return categoryAllowed;
        }

        foreach (var productNotAllowed in ProductsFromCategoryNotAllowed)
        {
            yield return productNotAllowed;
        }
    }
}
