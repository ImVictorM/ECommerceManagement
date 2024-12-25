using Application.Orders.Commands.PlaceOrder;
using Application.Orders.Queries.GetOrderById;
using Carter;
using Contracts.Orders;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization.CustomerRequired;
using WebApi.Common.Extensions;

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
            .RequireAuthorization(CustomerRequiredPolicy.Name);

        orderGroup
            .MapGet("/{id:long}", GetOrderById)
            .WithName("GetOrderById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Order By Id",
                Description = "Retrieves an order by identifier. Customers can only retrieve their own orders. Administrators can access any order."
            })
            .RequireAuthorization();
    }

    private async Task<Results<Created, UnauthorizedHttpResult, BadRequest>> PlaceOrder(
        [FromBody] PlaceOrderRequest request,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ISender sender
    )
    {
        var authenticatedUserId = httpContextAccessor.HttpContext!.User.GetId();

        var command = mapper.Map<PlaceOrderCommand>((authenticatedUserId, request));

        var response = await sender.Send(command);

        return TypedResults.Created($"/{BaseEndpoint}/{response.Id}");
    }

    private async Task<Results<Ok<OrderDetailedResponse>, ForbidHttpResult, NotFound>> GetOrderById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper,
        IHttpContextAccessor contextAccessor
    )
    {
        var authenticatedUserId = contextAccessor.HttpContext!.User.GetId();

        var query = new GetOrderByIdQuery(authenticatedUserId, id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<OrderDetailedResponse>(result));
    }
}
