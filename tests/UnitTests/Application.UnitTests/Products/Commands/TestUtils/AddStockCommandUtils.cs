using Application.Products.Commands.AddStock;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Products.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="AddStockCommand"/> command.
/// </summary>
public static class AddStockCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="AddStockCommand"/> class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantityToAdd">The quantity to add.</param>
    /// <returns>
    /// A new instance of the <see cref="AddStockCommand"/> class.
    /// </returns>
    public static AddStockCommand CreateCommand(
        string? productId = null,
        int quantityToAdd = 1
    )
    {
        return new AddStockCommand(
            productId ?? NumberUtils.CreateRandomLongAsString(),
            quantityToAdd
        );
    }
}
