using Contracts.Common;

namespace Contracts.Products;

/// <summary>
/// Request to update a product's discount list.
/// </summary>
/// <param name="Discounts">The new list of discounts.</param>
public record UpdateProductDiscountsRequest(IEnumerable<Discount> Discounts);
