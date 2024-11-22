using System.Linq.Expressions;
using SharedKernel.Abstracts;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Specification to get products that contains certain categories.
/// </summary>
public class QueryProductByCategorySpec : CompositeQuerySpecification<Product>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="QueryProductByCategorySpec"/> class.
    /// </summary>
    /// <param name="categoryIds">The categories a product may contain.</param>
    public QueryProductByCategorySpec(IEnumerable<long> categoryIds) : base(CreateCriteria(categoryIds))
    {
    }

    private static Expression<Func<Product, bool>> CreateCriteria(IEnumerable<long> categoryIds)
    {
        return product => product.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId));
    }
}
