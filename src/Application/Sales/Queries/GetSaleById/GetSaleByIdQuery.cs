using Application.Common.Security.Authorization.Requests;
using Application.Sales.DTOs;

using SharedKernel.ValueObjects;

namespace Application.Sales.Queries.GetSaleById;

/// <summary>
/// Represents a query to retrieve a sale by its identifier.
/// </summary>
/// <param name="SaleId">The sale identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetSaleByIdQuery(string SaleId)
    : IRequestWithAuthorization<SaleResult>;
