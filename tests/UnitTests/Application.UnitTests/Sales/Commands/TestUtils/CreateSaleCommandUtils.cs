using Application.Sales.Commands.CreateSale;

using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Application.UnitTests.Sales.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateSaleCommand"/> class.
/// </summary>
public static class CreateSaleCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="CreateSaleCommand"/> class.
    /// </summary>
    /// <param name="discount">The new sale discount.</param>
    /// <param name="categoryOnSaleIds">
    /// The new category on sale identifiers.
    /// </param>
    /// <param name="productOnSaleIds">
    /// The product on sale identifiers.
    /// </param>
    /// <param name="productExcludedFromSaleIds">
    /// The product excluded from sale identifiers.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="CreateSaleCommand"/> class.
    /// </returns>
    /// <remarks>
    /// If the <paramref name="productOnSaleIds"/> and
    /// <paramref name="categoryOnSaleIds"/> parameters are not defined,
    /// a sale containing a random product identifier on sale will be created to
    /// guarantee a valid sale is created.
    /// </remarks>
    public static CreateSaleCommand CreateCommand(
        Discount? discount = null,
        IEnumerable<string>? categoryOnSaleIds = null,
        IEnumerable<string>? productOnSaleIds = null,
        IEnumerable<string>? productExcludedFromSaleIds = null
    )
    {
        var productsOnSaleIdsOrDefault =
            categoryOnSaleIds == null && productOnSaleIds == null
                ? [NumberUtils.CreateRandomLongAsString()]
                : productOnSaleIds ?? [];

        return new CreateSaleCommand(
            discount ?? DiscountUtils.CreateDiscountValidToDate(),
            categoryOnSaleIds ?? [],
            productsOnSaleIdsOrDefault,
            productExcludedFromSaleIds ?? []
        );
    }
}
