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
    /// <param name="discount">The sale discount.</param>
    /// <param name="categoryOnSaleIds">The category on sale ids.</param>
    /// <param name="productOnSaleIds">The product on sale ids.</param>
    /// <param name="productExcludedFromSaleIds">
    /// The product excluded from sale ids.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="CreateSaleCommand"/> class.
    /// </returns>
    /// <remarks>
    /// If the <paramref name="productOnSaleIds"/> and
    /// <paramref name="categoryOnSaleIds"/> parameters are empty, a sale containing
    /// a random product id on sale will be created to guarantee a valid sale
    /// is created.
    /// </remarks>
    public static CreateSaleCommand CreateCommand(
        Discount? discount = null,
        IEnumerable<string>? categoryOnSaleIds = null,
        IEnumerable<string>? productOnSaleIds = null,
        IEnumerable<string>? productExcludedFromSaleIds = null
    )
    {
        var productsOnSaleIdsOrDefault =
            IsSaleCreationParametersEmpty(categoryOnSaleIds, productOnSaleIds)
                ? [NumberUtils.CreateRandomLongAsString()]
                : productOnSaleIds!;

        return new CreateSaleCommand(
            discount ?? DiscountUtils.CreateDiscountValidToDate(),
            categoryOnSaleIds ?? [],
            productsOnSaleIdsOrDefault,
            productExcludedFromSaleIds ?? []
        );
    }

    private static bool IsSaleCreationParametersEmpty(
        IEnumerable<string>? categoryOnSaleIds = null,
        IEnumerable<string>? productOnSaleIds = null
    )
    {
        return (categoryOnSaleIds == null || !categoryOnSaleIds.Any())
            && (productOnSaleIds == null || !productOnSaleIds.Any());
    }
}
