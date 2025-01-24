using System.Linq.Expressions;
using Domain.CategoryAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Specification to get products that contain all specified categories.
/// </summary>
public class QueryProductsContainingCategoriesSpecification : CompositeQuerySpecification<Product>
{
    private readonly IEnumerable<CategoryId> _categoryIds;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryProductsContainingCategoriesSpecification"/> class.
    /// </summary>
    /// <param name="categoryIds">The categories a product must contain.</param>
    public QueryProductsContainingCategoriesSpecification(IEnumerable<CategoryId> categoryIds) : base()
    {
        _categoryIds = categoryIds;
    }

    /// <inheritdoc/>
    public override Expression<Func<Product, bool>> Criteria =>
        product => product.ProductCategories
            .Where(pc => _categoryIds.Contains(pc.CategoryId))
            .Select(pc => pc.CategoryId)
            .Distinct()
            .Count() == _categoryIds.Count();
}
