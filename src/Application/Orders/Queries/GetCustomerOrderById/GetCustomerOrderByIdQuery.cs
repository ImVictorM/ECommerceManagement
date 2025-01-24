using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using Application.Orders.Common.DTOs;

namespace Application.Orders.Queries.GetCustomerOrderById;

/// <summary>
/// Represents a query to get a customer order by id.
/// </summary>
/// <param name="UserId">The order owner id.</param>
/// <param name="OrderId">The order id.</param>
[Authorize(policyType: (typeof(SelfOrAdminPolicy)))]
public record GetCustomerOrderByIdQuery(string UserId, string OrderId) : IRequestWithAuthorization<OrderDetailedResult>;
