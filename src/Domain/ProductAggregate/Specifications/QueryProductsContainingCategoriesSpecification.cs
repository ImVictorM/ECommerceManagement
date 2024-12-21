using System.Linq.Expressions;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Specification to get products that contains certain categories.
/// </summary>
public class QueryProductsContainingCategoriesSpecification : CompositeQuerySpecification<Product>
{
    private readonly HashSet<ProductCategory> _categories;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryProductsContainingCategoriesSpecification"/> class.
    /// </summary>
    /// <param name="categories">The categories a product may contain.</param>
    public QueryProductsContainingCategoriesSpecification(HashSet<ProductCategory> categories) : base()
    {
        _categories = categories;
    }

    /// <inheritdoc/>
    public override Expression<Func<Product, bool>> Criteria => product => _categories.All(category => product.ProductCategories.Contains(category));
}
