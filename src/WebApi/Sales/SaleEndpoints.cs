using Application.Sales.Commands.CreateSale;

using Contracts.Sales;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Carter;
using MapsterMapper;
using MediatR;

namespace WebApi.Sales;

/// <summary>
/// Provides endpoints for the sale features.
/// </summary>
public sealed class SaleEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var saleGroup = app
            .MapGroup("/sales")
            .WithTags("Sales")
            .WithOpenApi();

        saleGroup
            .MapPost("/", CreateSale)
            .WithName(nameof(CreateSale))
            .WithOpenApi(op => new(op)
            {
                Summary = "Create Sale",
                Description =
                "Creates a new sale, defining the products and/or categories. " +
                "Admin authentication is required."
            })
            .RequireAuthorization();
    }

    internal async Task<Results<
        Created,
        BadRequest,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> CreateSale(
        [FromBody] CreateSaleRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<CreateSaleCommand>(request);

        var result = await sender.Send(command);

        return TypedResults.Created($"/sales/{result.Id}");
    }
}
