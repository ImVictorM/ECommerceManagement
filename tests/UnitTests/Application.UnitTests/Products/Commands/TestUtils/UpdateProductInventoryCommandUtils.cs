using Application.Products.Commands.UpdateProductInventory;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Products.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateProductInventoryCommand"/> command.
/// </summary>
public static class UpdateProductInventoryCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateProductInventoryCommand"/> class.
    /// Defaults to 1 quantity to add.
    /// </summary>
    /// <param name="quantityToAdd">The quantity to add.</param>
    /// <param name="productId">The product id.</param>
    /// <returns>A new instance of the <see cref="UpdateProductInventoryCommand"/> class.</returns>
    public static UpdateProductInventoryCommand CreateCommand(
        string? productId = null,
        int quantityToAdd = 1
    )
    {
        return new UpdateProductInventoryCommand(
            productId ?? DomainConstants.Product.Id.ToString(),
            quantityToAdd
        );
    }
}
