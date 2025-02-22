using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.ValueObjects;

namespace Domain.SaleAggregate.Services;

/// <summary>
/// Define sale related services.
/// </summary>
public interface ISaleService
{
    /// <summary>
    /// Retrieves sales that apply to a given collection of products.
    /// </summary>
    /// <param name="products">The collection of products to check for applicable sales.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A dictionary where each <see cref="ProductId"/> key maps to a collection of <see cref="Sale"/> objects 
    /// that apply to the product.
    /// </returns>
    Task<IDictionary<ProductId, IEnumerable<Sale>>> GetProductsSalesAsync(
        IEnumerable<SaleProduct> products,
        CancellationToken cancellationToken = default
    );
}
