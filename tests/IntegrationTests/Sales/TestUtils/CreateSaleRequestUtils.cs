using Contracts.Common;
using Contracts.Sales;

using Domain.UnitTests.TestUtils;

using IntegrationTests.TestUtils.Contracts;

namespace IntegrationTests.Sales.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateSaleRequest"/> class.
/// </summary>
public static class CreateSaleRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="CreateSaleRequest"/> class.
    /// </summary>
    /// <param name="discount">The sale discount.</param>
    /// <param name="categoryOnSaleIds">The category on sale ids.</param>
    /// <param name="productOnSaleIds">The product on sale ids.</param>
    /// <param name="productExcludedFromSaleIds">
    /// The product excluded from sale ids.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="CreateSaleRequest"/> class.
    /// </returns>
    /// <remarks>
    /// If the <paramref name="productOnSaleIds"/> and
    /// <paramref name="categoryOnSaleIds"/> parameters are not defined, a sale containing
    /// a random product id on sale will be created to guarantee a valid sale
    /// is created.
    /// </remarks>
    public static CreateSaleRequest CreateRequest(
        DiscountContract? discount = null,
        IEnumerable<string>? categoryOnSaleIds = null,
        IEnumerable<string>? productOnSaleIds = null,
        IEnumerable<string>? productExcludedFromSaleIds = null
    )
    {
        var productsOnSaleIdsOrDefault =
            categoryOnSaleIds == null && productOnSaleIds == null
                ? [NumberUtils.CreateRandomLongAsString()]
                : productOnSaleIds ?? [];

        return new CreateSaleRequest(
            discount ?? DiscountContractUtils.CreateDiscountValidToDate(),
            categoryOnSaleIds ?? [],
            productsOnSaleIdsOrDefault,
            productExcludedFromSaleIds ?? []
        );
    }
}
