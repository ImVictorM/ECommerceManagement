using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using Application.Orders.DTOs.Filters;
using Application.Orders.DTOs.Results;

namespace Application.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Represents a query to fetch a customer's orders.
/// </summary>
/// <param name="UserId">The order owner identifier.</param>
/// <param name="Filters">The order filters.</param>
[Authorize(policyType: typeof(SelfOrAdminPolicy<GetCustomerOrdersQuery>))]
public record class GetCustomerOrdersQuery(
    string UserId,
    OrderFilters Filters
) : IRequestWithAuthorization<IReadOnlyList<OrderResult>>,
    IUserSpecificResource;
