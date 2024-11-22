using SharedKernel.Abstracts;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Query specification to get products that are active.
/// </summary>
public class QueryProductActiveSpec : CompositeQuerySpecification<Product>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="QueryProductActiveSpec"/>.
    /// </summary>
    public QueryProductActiveSpec() : base(product => product.IsActive)
    {
    }
}
