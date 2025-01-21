using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;
using Application.Orders.Common.DTOs;

namespace Application.Orders.Queries.GetOrders;

/// <summary>
/// Represents a query to fetch all orders.
/// </summary>
/// <param name="Status">The status filter condition.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetOrdersQuery(string? Status = null) : IRequestWithAuthorization<IEnumerable<OrderResult>>
{
    /// <inheritdoc/>
    public string? UserId => null;
}
