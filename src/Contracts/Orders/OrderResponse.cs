using Contracts.Common;

namespace Contracts.Orders;

/// <summary>
/// Represents an order response.
/// </summary>
/// <param name="Id">The order id.</param>
/// <param name="OwnerId">The order owner id.</param>
/// <param name="BaseTotal">The order total without applying discounts.</param>
/// <param name="Description">The order description.</param>
/// <param name="Status">The order status.</param>
/// <param name="TotalWithDiscounts">The order total applyting discounts.</param>
public record OrderResponse(
    string Id,
    string OwnerId,
    decimal BaseTotal,
    string Description,
    string Status,
    decimal TotalWithDiscounts
    //IEnumerable<Discount> DiscountsApplied
);
