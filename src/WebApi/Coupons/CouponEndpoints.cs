using Application.Coupons.Commands.CreateCoupon;
using Application.Coupons.Commands.DeleteCoupon;
using Application.Coupons.Commands.ToggleCouponActivation;
using Application.Coupons.Commands.UpdateCoupon;
using Application.Coupons.Queries.GetCoupons;
using Application.Coupons.DTOs;

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
                "Toggles the active status of an existing coupon. " +
                "Admin authentication is required.",
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
                "Updates the details of an existing coupon. " +
                "Admin authentication is required.",
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
            .MapGet("/", GetCoupons)
            .WithName(nameof(GetCoupons))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Coupons",
                Description =
                "Retrieves a list of coupons based on the specified filters. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "active",
                        In = ParameterLocation.Query,
                        Description = "Filter coupons by their activation status.",
                        Required = false,
                        Schema = new() { Type = "boolean" }
                    },
                    new()
                    {
                        Name = "expiringAfter",
                        In = ParameterLocation.Query,
                        Description =
                        "Filter coupons that expire after the specified UTC date.",
                        Required = false,
                        Schema = new() { Type = "string", Format = "date-time" }
                    },
                    new()
                    {
                        Name = "expiringBefore",
                        In = ParameterLocation.Query,
                        Description =
                        "Filter coupons that expire before the specified UTC date.",
                        Required = false,
                        Schema = new() { Type = "string", Format = "date-time" }
                    },
                    new()
                    {
                        Name = "validForDate",
                        In = ParameterLocation.Query,
                        Description =
                        "Filter coupons that are valid on the specified UTC date.",
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

    internal async Task<Results<
        Ok<IEnumerable<CouponResponse>>,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> GetCoupons(
        ISender sender,
        IMapper mapper,
        [FromQuery(Name = "active")] bool? active = null,
        [FromQuery(Name = "expiringAfter")] DateTimeOffset? expiringAfter = null,
        [FromQuery(Name = "expiringBefore")] DateTimeOffset? expiringBefore = null,
        [FromQuery(Name = "validForDate")] DateTimeOffset? validForDate = null
    )
    {
        var query = new GetCouponsQuery(new CouponFilters(
            active,
            expiringAfter,
            expiringBefore,
            validForDate
        ));

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<CouponResponse>));
    }
}
