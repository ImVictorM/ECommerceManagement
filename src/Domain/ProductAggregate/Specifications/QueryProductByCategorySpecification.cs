using System.Linq.Expressions;
using SharedKernel.Models;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Specification to get products that contains certain categories.
/// </summary>
public class QueryProductByCategorySpecification : CompositeQuerySpecification<Product>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="QueryProductByCategorySpecification"/> class.
    /// </summary>
    /// <param name="categoryIds">The categories a product may contain.</param>
    public QueryProductByCategorySpecification(IEnumerable<long> categoryIds) : base(CreateCriteria(categoryIds))
    {
    }

    private static Expression<Func<Product, bool>> CreateCriteria(IEnumerable<long> categoryIds)
    {
        return product => product.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId));
    }
}
