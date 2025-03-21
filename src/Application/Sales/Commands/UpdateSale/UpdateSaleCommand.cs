using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Sales.Commands.UpdateSale;

/// <summary>
/// Represents a command to update a sale.
/// </summary>
/// <param name="SaleId">The sale to be updated identifier.</param>
/// <param name="Discount">The new sale discount.</param>
/// <param name="CategoryOnSaleIds">The new category on sale ids.</param>
/// <param name="ProductOnSaleIds">The new product on sale ids.</param>
/// <param name="ProductExcludedFromSaleIds">
/// The new product excluded from sale ids.
/// </param>
[Authorize(roleName: nameof(Role.Admin))]
public record UpdateSaleCommand(
    string SaleId,
    Discount Discount,
    IEnumerable<string> CategoryOnSaleIds,
    IEnumerable<string> ProductOnSaleIds,
    IEnumerable<string> ProductExcludedFromSaleIds
) : IRequestWithAuthorization<Unit>;
