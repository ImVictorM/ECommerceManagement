using Application.Common.Security.Authorization.Requests;
using Application.Orders.DTOs;
using SharedKernel.ValueObjects;

namespace Application.Orders.Queries.GetOrders;

/// <summary>
/// Represents a query to fetch all orders.
/// </summary>
/// <param name="Status">The status filter condition.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetOrdersQuery(string? Status = null) : IRequestWithAuthorization<IEnumerable<OrderResult>>;
