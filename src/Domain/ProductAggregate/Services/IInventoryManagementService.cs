using Domain.ProductAggregate.ValueObjects;

namespace Domain.ProductAggregate.Services;

/// <summary>
/// Provides inventory management operations.
/// </summary>
public interface IInventoryManagementService
{
    /// <summary>
    /// Reserves inventory for a given list of products.
    /// </summary>
    /// <param name="productsReserved">
    /// The collection of products to reserve in inventory.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A collection of reserved products with updated inventory.
    /// </returns>
    Task<IEnumerable<Product>> ReserveInventoryAsync(
        IEnumerable<ProductReserved> productsReserved,
        CancellationToken cancellationToken = default
    );
}
