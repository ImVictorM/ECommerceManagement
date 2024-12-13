using System.Linq.Expressions;
using SharedKernel.Models;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Specification to get products that contains certain categories.
/// </summary>
public class QueryProductByCategorySpecification : CompositeQuerySpecification<Product>
{
    private readonly IEnumerable<long> _categoryIds;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryProductByCategorySpecification"/> class.
    /// </summary>
    /// <param name="categoryIds">The categories a product may contain.</param>
    public QueryProductByCategorySpecification(IEnumerable<long> categoryIds) : base()
    {
        _categoryIds = categoryIds;
    }

    /// <inheritdoc/>
    public override Expression<Func<Product, bool>> Criteria => product => product.ProductCategories.Any(pc => _categoryIds.Contains(pc.CategoryId));

}
