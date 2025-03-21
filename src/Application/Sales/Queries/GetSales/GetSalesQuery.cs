using Application.Common.Security.Authorization.Requests;
using Application.Sales.DTOs;

using SharedKernel.ValueObjects;

namespace Application.Sales.Queries.GetSales;

/// <summary>
/// Represents a query to retrieve sales based on specified filters.
/// </summary>
/// <param name="Filters">The filters to apply when querying the sales.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetSalesQuery(SaleFilters Filters)
    : IRequestWithAuthorization<IReadOnlyList<SaleResult>>;
