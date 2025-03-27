using Application.ProductReviews.Queries.GetCustomerProductReviews;
using Application.ProductReviews.Commands.DeactivateCustomerProductReview;

using Contracts.ProductReviews;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.ProductReviews;

/// <summary>
/// Provides endpoints for the customer product review features.
/// </summary>
public class CustomerProductReviewEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/users/customers/{userId:long}/reviews")
            .WithTags("ProductReviews")
            .WithOpenApi();

        group
            .MapGet("/", GetCustomerProductReviews)
            .WithName(nameof(GetCustomerProductReviews))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Customer Product Reviews",
                Description =
                "Retrieves a list of active product reviews left " +
                "by a specific customer. " +
                "Customer or admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "userId",
                        In = ParameterLocation.Path,
                        Description = "The customer identifier.",
                        Required = true,
                        Schema = new()
                        {
                            Type = "integer",
                            Format = "int64"
                        }
                    }
                ],
            })
            .RequireAuthorization();

        group
            .MapDelete("/{reviewId:long}", DeactivateCustomerProductReviews)
            .WithName(nameof(DeactivateCustomerProductReviews))
            .WithOpenApi(op => new(op)
            {
                Summary = "Deactivate Customer Product Review",
                Description =
                "Deactivates a specific product review from a customer. " +
                "Customer or admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "userId",
                        In = ParameterLocation.Path,
                        Description = "The customer identifier.",
                        Required = true,
                        Schema = new()
                        {
                            Type = "integer",
                            Format = "int64"
                        }
                    },
                    new()
                    {
                        Name = "reviewId",
                        In = ParameterLocation.Path,
                        Description = "The review identifier.",
                        Required = true,
                        Schema = new()
                        {
                            Type = "integer",
                            Format = "int64"
                        }
                    }
                ],
            })
            .RequireAuthorization();
    }

    internal async Task<Results<
        Ok<IEnumerable<ProductReviewResponse>>,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> GetCustomerProductReviews(
        [FromRoute] string userId,
        IMapper mapper,
        ISender sender
    )
    {
        var query = new GetCustomerProductReviewsQuery(userId);

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<ProductReviewResponse>));
    }

    internal async Task<Results<
        NoContent,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> DeactivateCustomerProductReviews(
        [FromRoute] string userId,
        [FromRoute] string reviewId,
        ISender sender
    )
    {
        var query = new DeactivateCustomerProductReviewCommand(
            userId,
            reviewId
        );

        await sender.Send(query);

        return TypedResults.NoContent();
    }
}
