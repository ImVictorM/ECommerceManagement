using Application.Orders.Queries.GetCustomerOrderById;
using Application.Orders.Queries.GetCustomerOrders;

using Contracts.Orders;

using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Orders;

/// <summary>
/// Defines all endpoints related to user orders.
/// </summary>
public sealed class UserOrderEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var userOrderGroup = app
            .MapGroup("users/{userId:long}/orders")
            .WithTags("Orders")
            .WithOpenApi();

        userOrderGroup
            .MapGet("/", GetCustomerOrders)
            .WithName("GetCustomerOrders")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Customer Orders",
                Description = "Retrieves a customer orders. Requires authentication as order owner or administrator."
            })
            .RequireAuthorization();

        userOrderGroup
            .MapGet("/{orderId:long}", GetCustomerOrderById)
            .WithName("GetCustomerOrderById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Customer Order By Id",
                Description = "Retrieves a customer's order by its identifier. Requires authentication as order owner or administrator."
            })
            .RequireAuthorization();
    }

    private async Task<Results<Ok<IEnumerable<OrderResponse>>, ForbidHttpResult, UnauthorizedHttpResult>> GetCustomerOrders(
        [FromRoute] string userId,
        [FromQuery(Name = "status")] string? status,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetCustomerOrdersQuery(userId, status);

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<OrderResponse>));
    }

    private async Task<Results<Ok<OrderDetailedResponse>, ForbidHttpResult, UnauthorizedHttpResult>> GetCustomerOrderById(
        [FromRoute] string userId,
        [FromRoute] string orderId,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetCustomerOrderByIdQuery(userId, orderId);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<OrderDetailedResponse>(result));
    }
}
