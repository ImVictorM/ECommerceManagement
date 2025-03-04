using Application.ProductFeedback.Commands.LeaveProductFeedback;
using Application.ProductFeedback.Queries.GetProductFeedback;

using Contracts.ProductFeedback;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.ProductFeedback;

/// <summary>
/// Provides endpoints for the product feedback features.
/// </summary>
public class ProductFeedbackEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var productFeedbackGroup = app
            .MapGroup("/products/{productId:long}/feedback")
            .WithTags("ProductFeedback")
            .WithOpenApi();

        productFeedbackGroup
            .MapPost("/", LeaveProductFeedback)
            .WithName(nameof(LeaveProductFeedback))
            .WithOpenApi(op => new(op)
            {
                Summary = "Leave Product Feedback",
                Description =
                "Allows a customer to leave feedback for a purchased product. " +
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

        productFeedbackGroup
            .MapGet("/", GetProductFeedback)
            .WithName(nameof(GetProductFeedback))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Product Feedback",
                Description =
                "Retrieves a list of active feedback items for a specific product.",
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
    >> LeaveProductFeedback(
        [FromRoute] string productId,
        [FromBody] LeaveProductFeedbackRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<LeaveProductFeedbackCommand>((productId, request));

        var result = await sender.Send(command);

        return TypedResults.Created($"/products/{productId}/feedback/{result.Id}");
    }

    internal async Task<Ok<IEnumerable<ProductFeedbackDetailedResponse>>> GetProductFeedback(
        [FromRoute] string productId,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetProductFeedbackQuery(productId);

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<ProductFeedbackDetailedResponse>));
    }
}
