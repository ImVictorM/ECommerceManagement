using Application.Coupons.Commands.CreateCoupon;

using Contracts.Coupons;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
}
