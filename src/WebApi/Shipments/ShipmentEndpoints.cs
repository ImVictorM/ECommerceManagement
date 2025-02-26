using Application.Shipments.Commands.AdvanceShipmentStatus;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MediatR;
using Carter;

namespace WebApi.Shipments;

/// <summary>
/// Provides endpoints for the shipment features.
/// </summary>
public sealed class ShipmentEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var shipmentsGroup = app
            .MapGroup("shipments")
            .WithTags("Shipments")
            .WithOpenApi();

        shipmentsGroup
            .MapPatch("/{id:long}/status", AdvanceShipmentStatus)
            .WithName("AdvanceShipmentStatus")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Advance Shipment Status",
                Description = "Advances a shipment status to the next state. Carrier authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The shipment identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    },
                ],
            })
            .RequireAuthorization();
    }

    private async Task<Results<NoContent, BadRequest, NotFound, UnauthorizedHttpResult, ForbidHttpResult>> AdvanceShipmentStatus(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new AdvanceShipmentStatusCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
