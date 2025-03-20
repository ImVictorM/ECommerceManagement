using Contracts.Common;

namespace Contracts.Sales;

/// <summary>
/// Represents a request to create a new sale.
/// </summary>
/// <param name="Discount">The sale discount.</param>
/// <param name="CategoryOnSaleIds">The category on sale ids.</param>
/// <param name="ProductOnSaleIds">The product on sale ids.</param>
/// <param name="ProductExcludedFromSaleIds">
/// The product excluded from sale ids.
/// </param>
public record CreateSaleRequest(
    DiscountContract Discount,
    IEnumerable<string> CategoryOnSaleIds,
    IEnumerable<string> ProductOnSaleIds,
    IEnumerable<string> ProductExcludedFromSaleIds
);
