using Application.Orders.Commands.PlaceOrder;
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
}
