using Application.Common.Security.Authorization.Requests;
using Application.Orders.DTOs.Filters;
using Application.Orders.DTOs.Results;

using SharedKernel.ValueObjects;

namespace Application.Orders.Queries.GetOrders;

/// <summary>
/// Represents a query to fetch all the orders with filtering support.
/// </summary>
/// <param name="Filters">The filtering criteria.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetOrdersQuery(OrderFilters Filters)
    : IRequestWithAuthorization<IReadOnlyList<OrderResult>>;
