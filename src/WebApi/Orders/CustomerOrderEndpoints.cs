using Application.Orders.Queries.GetCustomerOrderById;
using Application.Orders.Queries.GetCustomerOrders;

using Contracts.Orders;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.Orders;

/// <summary>
/// Provides endpoints for the customer order features.
/// </summary>
public sealed class CustomerOrderEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var userOrderGroup = app
            .MapGroup("users/customers/{userId:long}/orders")
            .WithTags("Orders")
            .WithOpenApi();

        userOrderGroup
            .MapGet("/", GetCustomerOrders)
            .WithName("GetCustomerOrders")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Customer Orders",
                Description = "Retrieves a customer orders. Requires authentication as order owner or administrator.",
                Parameters =
                [
                    new()
                    {
                        Name = "userId",
                        In = ParameterLocation.Path,
                        Description = "The customer identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    },
                    new()
                    {
                        Name = "status",
                        In = ParameterLocation.Query,
                        Description = "Filters orders by status.",
                        Required = false,
                        Schema = new() { Type = "string" }
                    }
                ],
            })
            .RequireAuthorization();

        userOrderGroup
            .MapGet("/{orderId:long}", GetCustomerOrderById)
            .WithName("GetCustomerOrderById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Customer Order By Id",
                Description = "Retrieves a customer's order by its identifier. Requires authentication as order owner or administrator.",
                Parameters =
                [
                    new()
                    {
                        Name = "userId",
                        In = ParameterLocation.Path,
                        Description = "The customer identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    },
                    new()
                    {
                        Name = "orderId",
                        In = ParameterLocation.Path,
                        Description = "The order identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    },
                ],
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
