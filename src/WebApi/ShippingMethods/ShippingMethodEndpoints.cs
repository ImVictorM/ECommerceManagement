using Application.ShippingMethods.Commands.CreateShippingMethod;
using Application.ShippingMethods.Commands.UpdateShippingMethod;
using Application.ShippingMethods.Commands.DeleteShippingMethod;
using Application.ShippingMethods.Queries.GetShippingMethodById;
using Application.ShippingMethods.Queries.GetShippingMethods;

using Contracts.ShippingMethods;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.ShippingMethods;

/// <summary>
/// Provides endpoints for the shipping method features.
/// </summary>
public sealed class ShippingMethodEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/shipping/methods")
            .WithTags("ShippingMethods")
            .WithOpenApi();

        group
            .MapPost("/", CreateShippingMethod)
            .WithName(nameof(CreateShippingMethod))
            .WithOpenApi(op => new(op)
            {
                Summary = "Create Shipping Method",
                Description =
                "Creates a new shipping method. " +
                "Admin authentication is required."
            })
            .RequireAuthorization();

        group
            .MapPut("/{id:long}", UpdateShippingMethod)
            .WithName(nameof(UpdateShippingMethod))
            .WithOpenApi(op => new(op)
            {
                Summary = "Update Shipping Method",
                Description =
                "Updates a shipping method. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The shipping method identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            })
            .RequireAuthorization();

        group
            .MapDelete("/{id:long}", DeleteShippingMethod)
            .WithName(nameof(DeleteShippingMethod))
            .WithOpenApi(op => new(op)
            {
                Summary = "Delete Shipping Method",
                Description =
                "Deletes a shipping method by its identifier. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The shipping method identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            })
            .RequireAuthorization();

        group
            .MapGet("/{id:long}", GetShippingMethodById)
            .WithName(nameof(GetShippingMethodById))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Shipping Method By Id",
                Description = "Retrieves a shipping method by its identifier.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The shipping method identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            });

        group
            .MapGet("/", GetShippingMethods)
            .WithName(nameof(GetShippingMethods))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Shipping Methods",
                Description = "Retrieves all the available shipping methods."
            });
    }

    internal async Task<Results<
        Created,
        BadRequest,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> CreateShippingMethod(
        [FromBody] CreateShippingMethodRequest request,
        IMapper mapper,
        ISender sender
    )
    {
        var command = mapper.Map<CreateShippingMethodCommand>(request);

        var response = await sender.Send(command);

        return TypedResults.Created($"/shipping/methods/{response.Id}");
    }

    internal async Task<Results<
        NoContent,
        BadRequest,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> UpdateShippingMethod(
        [FromRoute] string id,
        [FromBody] UpdateShippingMethodRequest request,
        IMapper mapper,
        ISender sender
    )
    {
        var command = mapper.Map<UpdateShippingMethodCommand>((id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    internal async Task<Results<
        NoContent,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> DeleteShippingMethod(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new DeleteShippingMethodCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    internal async Task<Results<
        Ok<ShippingMethodResponse>,
        NotFound
    >> GetShippingMethodById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper)
    {
        var query = new GetShippingMethodByIdQuery(id);

        var response = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<ShippingMethodResponse>(response));
    }

    internal async Task<Ok<List<ShippingMethodResponse>>> GetShippingMethods(
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetShippingMethodsQuery();

        var result = await sender.Send(query);

        var response = result
            .Select(mapper.Map<ShippingMethodResponse>)
            .ToList();

        return TypedResults.Ok(response);
    }
}
