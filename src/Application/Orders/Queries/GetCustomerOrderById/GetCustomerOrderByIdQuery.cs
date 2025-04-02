using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using Application.Orders.DTOs.Results;

namespace Application.Orders.Queries.GetCustomerOrderById;

/// <summary>
/// Represents a query to get a customer's order.
/// </summary>
/// <param name="UserId">The order owner identifier.</param>
/// <param name="OrderId">The order identifier.</param>
[Authorize(policyType: (typeof(SelfOrAdminPolicy<GetCustomerOrderByIdQuery>)))]
public record GetCustomerOrderByIdQuery(string UserId, string OrderId) :
    IRequestWithAuthorization<OrderDetailedResult>,
    IUserSpecificResource;
