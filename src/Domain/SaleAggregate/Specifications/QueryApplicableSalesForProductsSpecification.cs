using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Models;

using System.Linq.Expressions;

namespace Domain.SaleAggregate.Specifications;

/// <summary>
/// Query specification to retrieve applicable sales for a collection of
/// products.
/// </summary>
public class QueryApplicableSalesForProductsSpecification
    : CompositeQuerySpecification<Sale>
{
    private readonly HashSet<ProductId> _productIds;
    private readonly HashSet<CategoryId> _categoryIds;

    /// <inheritdoc/>
    public override Expression<Func<Sale, bool>> Criteria => sale =>
        (
            sale.ProductsOnSale.Any(p => _productIds.Contains(p.ProductId))
            || sale.CategoriesOnSale.Any(c => _categoryIds.Contains(c.CategoryId))
        )
        && !sale.ProductsExcludedFromSale.Any(p => _productIds.Contains(p.ProductId))
        && (
            sale.Discount.StartingDate <= DateTimeOffset.UtcNow
            && sale.Discount.EndingDate >= DateTimeOffset.UtcNow
        );

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="QueryApplicableSalesForProductsSpecification"/> class.
    /// </summary>
    /// <param name="saleProducts">The sale products.</param>
    public QueryApplicableSalesForProductsSpecification(
        IEnumerable<SaleEligibleProduct> saleProducts
    )
    {
        var productList = saleProducts.ToList();

        _productIds = productList
            .Select(p => p.ProductId)
            .ToHashSet();

        _categoryIds = productList
            .SelectMany(p => p.CategoryIds)
            .ToHashSet();
    }
}
