using Contracts.Products;

namespace IntegrationTests.Products.TestUtils;

/// <summary>
/// Utilities for the <see cref="AddStockRequest"/> class.
/// </summary>
public static class AddStockRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="AddStockRequest"/>
    /// request.
    /// </summary>
    /// <param name="quantityToAdd">
    /// The quantity to add.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="AddStockRequest"/> class.
    /// </returns>
    public static AddStockRequest CreateRequest(
        int quantityToAdd = 1
    )
    {
        return new AddStockRequest(quantityToAdd);
    }
}
