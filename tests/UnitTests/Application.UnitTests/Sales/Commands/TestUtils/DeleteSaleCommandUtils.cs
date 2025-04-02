using Application.Sales.Commands.DeleteSale;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Sales.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="DeleteSaleCommand"/> class.
/// </summary>
public static class DeleteSaleCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeleteSaleCommand"/> class.
    /// </summary>
    /// <param name="saleId">The sale identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="DeleteSaleCommand"/> class.
    /// </returns>
    public static DeleteSaleCommand CreateCommand(
        string? saleId = null
    )
    {
        return new DeleteSaleCommand(
            saleId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
