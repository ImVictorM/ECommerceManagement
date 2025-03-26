using Application.Common.Security.Authorization.Requests;
using Application.Orders.DTOs.Results;
using SharedKernel.ValueObjects;

namespace Application.Orders.Queries.GetOrderById;

/// <summary>
/// Query to retrieve an order by its identifier.
/// </summary>
/// <param name="OrderId">The order identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetOrderByIdQuery(string OrderId)
    : IRequestWithAuthorization<OrderDetailedResult>;
