using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

using System.Linq.Expressions;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Defines a specification to query products by the specified identifiers.
/// </summary>
public class QueryProductsContainingIdsSpecification
    : CompositeQuerySpecification<Product>
{
    private readonly IReadOnlyList<ProductId> _productIds;

    /// <inheritdoc/>
    public override Expression<Func<Product, bool>> Criteria =>
        p => _productIds.Contains(p.Id);

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="QueryProductsContainingIdsSpecification"/> class.
    /// </summary>
    /// <param name="productIds">The product identifiers.</param>
    public QueryProductsContainingIdsSpecification(
        IEnumerable<ProductId> productIds
    )
    {
        _productIds = productIds.ToList();
    }
}
