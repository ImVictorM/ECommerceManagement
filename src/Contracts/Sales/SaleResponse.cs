using Contracts.Common;

namespace Contracts.Sales;

/// <summary>
/// Represents a sale response.
/// </summary>
/// <param name="Id">The sale identifier.</param>
/// <param name="Discount">The sale discount.</param>
/// <param name="CategoryOnSaleIds">The category on sale identifiers.</param>
/// <param name="ProductOnSaleIds">The product on sale identifiers.</param>
/// <param name="ProductExcludedFromSaleIds">
/// The product excluded from sale identifiers.
/// </param>
public record SaleResponse(
    string Id,
    DiscountContract Discount,
    IReadOnlyList<string> CategoryOnSaleIds,
    IReadOnlyList<string> ProductOnSaleIds,
    IReadOnlyList<string> ProductExcludedFromSaleIds
);
