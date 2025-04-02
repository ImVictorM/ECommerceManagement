using Contracts.Common;

namespace Contracts.Sales;

/// <summary>
/// Represents a request to update an existent sale.
/// </summary>
/// <param name="Discount">The new sale discount.</param>
/// <param name="CategoryOnSaleIds">
/// The new category on sale identifiers.
/// </param>
/// <param name="ProductOnSaleIds">
/// The new product on sale identifiers.
/// </param>
/// <param name="ProductExcludedFromSaleIds">
/// The new product excluded from sale identifiers.
/// </param>
public record UpdateSaleRequest(
    DiscountContract Discount,
    IEnumerable<string> CategoryOnSaleIds,
    IEnumerable<string> ProductOnSaleIds,
    IEnumerable<string> ProductExcludedFromSaleIds
);
