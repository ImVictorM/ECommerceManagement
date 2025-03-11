using Application.Coupons.Commands.CreateCoupon;
using Application.Coupons.Commands.DeleteCoupon;
using Application.Coupons.Commands.ToggleCouponActivation;
using Application.Coupons.Commands.UpdateCoupon;

using Contracts.Coupons;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.Coupons;

/// <summary>
/// Provides endpoints for the coupon features.
/// </summary>
public sealed class CouponEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var couponGroup = app
            .MapGroup("/coupons")
            .WithTags("Coupons")
            .WithOpenApi();

        couponGroup
            .MapPost("/", CreateCoupon)
            .WithName(nameof(CreateCoupon))
            .WithOpenApi(op => new(op)
            {
                Summary = "Create Coupon",
                Description =
                "Creates a new coupon. " +
                "Admin authentication is required."
            })
            .RequireAuthorization();

        couponGroup
            .MapDelete("/{id:long}", DeleteCoupon)
            .WithName(nameof(DeleteCoupon))
            .WithOpenApi(op => new(op)
            {
                Summary = "Delete Coupon",
                Description =
                "Deletes an existing coupon. Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The coupon identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    },
                ]
            })
            .RequireAuthorization();

        couponGroup
            .MapPatch("/{id:long}", ToggleCouponActivation)
            .WithName(nameof(ToggleCouponActivation))
            .WithOpenApi(op => new(op)
            {
                Summary = "Toggle Coupon Activation",
                Description =
                "Toggles a coupon active state. Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The coupon identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    },
                ]
            })
            .RequireAuthorization();

        couponGroup
            .MapPut("/{id:long}", UpdateCoupon)
            .WithName(nameof(UpdateCoupon))
            .WithOpenApi(op => new(op)
            {
                Summary = "Update Coupon",
                Description =
                "Updates an existing coupon. Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The coupon identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    },
                ]
            })
            .RequireAuthorization();
    }

    internal async Task<Results<
        Created,
        BadRequest,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> CreateCoupon(
        [FromBody] CreateCouponRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<CreateCouponCommand>(request);

        var result = await sender.Send(command);

        return TypedResults.Created($"/coupons/{result.Id}");
    }

    internal async Task<Results<
        NoContent,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> DeleteCoupon(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new DeleteCouponCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    internal async Task<Results<
        NoContent,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> ToggleCouponActivation(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new ToggleCouponActivationCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    internal async Task<Results<
        NoContent,
        BadRequest,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> UpdateCoupon(
        [FromRoute] string id,
        [FromBody] UpdateCouponRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<UpdateCouponCommand>((id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
