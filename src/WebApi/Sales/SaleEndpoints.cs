using Application.Sales.Commands.CreateSale;
using Application.Sales.Commands.DeleteSale;
using Application.Sales.Commands.UpdateSale;
using Application.Sales.Queries.GetSaleById;
using Application.Sales.Queries.GetSales;
using Application.Sales.DTOs;

using Contracts.Sales;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using Carter;
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

        saleGroup
            .MapDelete("/{id:long}", DeleteSale)
            .WithName(nameof(DeleteSale))
            .WithOpenApi(op => new(op)
            {
                Summary = "Delete Sale",
                Description =
                "Deletes an existing sale. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The sale identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ]
            })
            .RequireAuthorization();

        saleGroup
            .MapPut("/{id:long}", UpdateSale)
            .WithName(nameof(UpdateSale))
            .WithOpenApi(op => new(op)
            {
                Summary = "Update Sale",
                Description =
                "Updates an existing sale. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The sale identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ]
            })
            .RequireAuthorization();

        saleGroup
            .MapGet("/{id:long}", GetSaleById)
            .WithName(nameof(GetSaleById))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Sale By Id",
                Description =
                "Retrieves a sale by its identifier. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The sale identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ]
            })
            .RequireAuthorization();

        saleGroup
            .MapGet("/", GetSales)
            .WithName(nameof(GetSales))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Sales",
                Description =
                "Retrieves a list of sales based on the specified filters. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "expiringAfter",
                        In = ParameterLocation.Query,
                        Description =
                        "Filter sales that expire after the specified UTC date.",
                        Required = false,
                        Schema = new() { Type = "string", Format = "date-time" }
                    },
                    new()
                    {
                        Name = "expiringBefore",
                        In = ParameterLocation.Query,
                        Description =
                        "Filter sales that expire before the specified UTC date.",
                        Required = false,
                        Schema = new() { Type = "string", Format = "date-time" }
                    },
                    new()
                    {
                        Name = "validForDate",
                        In = ParameterLocation.Query,
                        Description =
                        "Filter sales that are valid on the specified UTC date.",
                        Required = false,
                        Schema = new() { Type = "string", Format = "date-time" }
                    }
                ]
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

    internal async Task<Results<
        NoContent,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> DeleteSale(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new DeleteSaleCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    internal async Task<Results<
        NoContent,
        BadRequest,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> UpdateSale(
        [FromBody] UpdateSaleRequest request,
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<UpdateSaleCommand>((id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    internal async Task<Results<
        Ok<SaleResponse>,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> GetSaleById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetSaleByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<SaleResponse>(result));
    }

    internal async Task<Results<
        Ok<List<SaleResponse>>,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> GetSales(
        ISender sender,
        IMapper mapper,
        [FromQuery(Name = "expiringAfter")] DateTimeOffset? expiringAfter = null,
        [FromQuery(Name = "expiringBefore")] DateTimeOffset? expiringBefore = null,
        [FromQuery(Name = "validForDate")] DateTimeOffset? validForDate = null
    )
    {
        var query = new GetSalesQuery(new SaleFilters(
            expiringAfter,
            expiringBefore,
            validForDate
        ));

        var result = await sender.Send(query);

        var response = result
            .Select(mapper.Map<SaleResponse>)
            .ToList();

        return TypedResults.Ok(response);
    }
}
