using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using Application.Orders.Common.DTOs;

namespace Application.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Represents a query to fetch a customer's orders.
/// </summary>
/// <param name="UserId">The customer (order owner) id.</param>
/// <param name="Status">(Optional) The status filter condition.</param>
[Authorize(policyType: typeof(SelfOrAdminPolicy))]
public record class GetCustomerOrdersQuery(string UserId, string? Status = null) : IRequestWithAuthorization<IEnumerable<OrderResult>>;
