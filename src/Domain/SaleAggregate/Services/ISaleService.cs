using Domain.SaleAggregate.ValueObjects;

namespace Domain.SaleAggregate.Services;

/// <summary>
/// Define sale related services.
/// </summary>
public interface ISaleService
{
    /// <summary>
    /// Retrieves the sales related to a product.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>A list of sales the product is in.</returns>
    Task<IEnumerable<Sale>> GetProductSalesAsync(SaleProduct product);
}
