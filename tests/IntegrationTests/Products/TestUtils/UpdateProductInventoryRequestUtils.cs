using Contracts.Products;

namespace IntegrationTests.Products.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateProductInventoryRequest"/> request.
/// </summary>
public static class UpdateProductInventoryRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateProductInventoryRequest"/> request.
    /// Defaults quantity to increment to 1.
    /// </summary>
    /// <param name="quantityToIncrement">The quantity to increment in the inventory.</param>
    /// <returns>A new instance of the <see cref="UpdateProductInventoryRequest"/> class.</returns>
    public static UpdateProductInventoryRequest CreateRequest(
        int? quantityToIncrement = null
    )
    {
        return new UpdateProductInventoryRequest(quantityToIncrement ?? 1);
    }
}
