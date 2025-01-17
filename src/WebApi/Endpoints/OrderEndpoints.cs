using Application.Orders.Commands.PlaceOrder;
using Application.Orders.Queries.GetOrderById;

using Contracts.Orders;

using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

/// <summary>
/// Defines order-related endpoints.
/// </summary>
public class OrderEndpoints : ICarterModule
{
    /// <summary>
    /// The base endpoint to access order resources.
    /// </summary>
    public const string BaseEndpoint = "orders";

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
                Description = "Places an order for a customer. Customer authentication required."
            })
            .RequireAuthorization();

        orderGroup
            .MapGet("/{id:long}", GetOrderById)
            .WithName("GetOrderById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Order By Id",
                Description = "Retrieves an order by identifier. Administrators can access any order."
            })
            .RequireAuthorization();
    }

    private async Task<Results<Created, UnauthorizedHttpResult, BadRequest, BadRequest<string>>> PlaceOrder(
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

    private async Task<Results<Ok<OrderDetailedResponse>, ForbidHttpResult, NotFound>> GetOrderById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetOrderByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<OrderDetailedResponse>(result));
    }
}
