using Application.ProductReviews.Queries.GetProductReviews;
using Application.ProductReviews.Commands.LeaveProductReview;

using Contracts.ProductReviews;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.ProductReviews;

/// <summary>
/// Provides endpoints for the product review features.
/// </summary>
public class ProductReviewEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/products/{productId:long}/reviews")
            .WithTags("ProductReviews")
            .WithOpenApi();

        group
            .MapPost("/", LeaveProductReview)
            .WithName(nameof(LeaveProductReview))
            .WithOpenApi(op => new(op)
            {
                Summary = "Leave Product Review",
                Description =
                "Allows a customer to leave a review for a purchased product. " +
                "Customer authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "productId",
                        In = ParameterLocation.Path,
                        Description = "The product identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            })
            .RequireAuthorization();

        group
            .MapGet("/", GetProductReviews)
            .WithName(nameof(GetProductReviews))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Product Reviews",
                Description =
                "Retrieves a list of active reviews for a specific product.",
                Parameters =
                [
                    new()
                    {
                        Name = "productId",
                        In = ParameterLocation.Path,
                        Description = "The product identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            });
    }

    internal async Task<Results<
        Created,
        BadRequest,
        ForbidHttpResult,
        UnauthorizedHttpResult
    >> LeaveProductReview(
        [FromRoute] string productId,
        [FromBody] LeaveProductReviewRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<LeaveProductReviewCommand>((productId, request));

        var result = await sender.Send(command);

        return TypedResults.Created($"/products/{productId}/reviews/{result.Id}");
    }

    internal async Task<Ok<List<ProductReviewDetailedResponse>>> GetProductReviews(
        [FromRoute] string productId,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetProductReviewsQuery(productId);

        var result = await sender.Send(query);

        var response = result
            .Select(mapper.Map<ProductReviewDetailedResponse>)
            .ToList();

        return TypedResults.Ok(response);
    }
}
