using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Specifications;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Query specification to retrieve an active product by its identifier.
/// </summary>
public class QueryActiveProductByIdSpecification
    : QueryActiveEntityByIdSpecification<Product, ProductId>
{
    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="QueryActiveProductByIdSpecification"/> class.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    public QueryActiveProductByIdSpecification(ProductId id) : base(id)
    {
    }
}
