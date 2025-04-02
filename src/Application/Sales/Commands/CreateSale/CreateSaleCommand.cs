using Application.Common.DTOs.Results;
using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

namespace Application.Sales.Commands.CreateSale;

/// <summary>
/// Represents a command to create a new sale.
/// </summary>
/// <param name="Discount">The sale discount.</param>
/// <param name="CategoryOnSaleIds">The category on sale identifiers.</param>
/// <param name="ProductOnSaleIds">The product on sale identifiers.</param>
/// <param name="ProductExcludedFromSaleIds">
/// The product excluded from sale identifiers.
/// </param>
[Authorize(roleName: nameof(Role.Admin))]
public record CreateSaleCommand(
    Discount Discount,
    IEnumerable<string> CategoryOnSaleIds,
    IEnumerable<string> ProductOnSaleIds,
    IEnumerable<string> ProductExcludedFromSaleIds
) : IRequestWithAuthorization<CreatedResult>;
