using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;
using Application.Orders.DTOs;

namespace Application.Orders.Queries.GetOrderById;

/// <summary>
/// Query to retrieve an order by id.
/// </summary>
/// <param name="OrderId">The order id.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetOrderByIdQuery(string OrderId) : IRequestWithAuthorization<OrderDetailedResult>;
