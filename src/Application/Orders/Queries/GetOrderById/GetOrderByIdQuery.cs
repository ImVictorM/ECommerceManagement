using Application.Orders.Common.DTOs;
using MediatR;

namespace Application.Orders.Queries.GetOrderById;

/// <summary>
/// Query to retrieve an order by id.
/// </summary>
/// <param name="CurrentUserId">The identifer of the current user.</param>
/// <param name="OrderId">The order id.</param>
public record GetOrderByIdQuery(string CurrentUserId, string OrderId) : IRequest<OrderResult>;
