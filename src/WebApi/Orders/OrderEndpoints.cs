using Application.Orders.Commands.PlaceOrder;
using Application.Orders.Queries.GetOrderById;
using Application.Orders.Queries.GetOrders;

using Contracts.Orders;

using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Orders;

/// <summary>
/// Defines order-related endpoints.
/// </summary>
public class OrderEndpoints : ICarterModule
{
    private const string BaseEndpoint = "orders";

    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var orderGroup = app
            .MapGroup(BaseEndpoint)
            .WithTags("Orders")
            .WithOpenApi();

        orderGroup
            .MapPost("/", PlaceOrder)
            .WithName("PlaceOrder")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Place Order",
                Description = "As a customer, places an order. Customer authentication is required."
            })
            .RequireAuthorization();

        orderGroup
            .MapGet("/{id:long}", GetOrderById)
            .WithName("GetOrderById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Order By Id",
                Description = "Retrieves an order by its identifier. Admin authentication is required."
            })
            .RequireAuthorization();

        orderGroup
            .MapGet("/", GetOrders)
            .WithName("GetOrders")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Orders",
                Description = "Retrieves all the orders. Admin authentication is required."
            })
            .RequireAuthorization();
    }

    private async Task<Results<Created, BadRequest, BadRequest<string>, ForbidHttpResult, UnauthorizedHttpResult>> PlaceOrder(
        [FromHeader(Name = "X-Idempotency-Key")] string requestId,
        [FromBody] PlaceOrderRequest request,
        IMapper mapper,
        ISender sender
    )
    {
        if (!Guid.TryParse(requestId, out var guidRequestId))
        {
            return TypedResults.BadRequest("Missing request unique identifier");
        }

        var command = mapper.Map<PlaceOrderCommand>((guidRequestId, request));

        var response = await sender.Send(command);

        return TypedResults.Created($"/{BaseEndpoint}/{response.Id}");
    }

    private async Task<Results<Ok<OrderDetailedResponse>, ForbidHttpResult, UnauthorizedHttpResult, NotFound>> GetOrderById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetOrderByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<OrderDetailedResponse>(result));
    }

    private async Task<Results<Ok<IEnumerable<OrderResponse>>, UnauthorizedHttpResult, ForbidHttpResult>> GetOrders(
        [FromQuery(Name = "status")] string? status,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetOrdersQuery(status);

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<OrderResponse>));
    }
}
