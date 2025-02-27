using Application.ProductFeedback.Commands.LeaveProductFeedback;

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
            .WithName("LeaveProductFeedback")
            .WithOpenApi(op => new(op)
            {
                Summary = "Leave Product Feedback",
                Description = "Allows a user to leave feedback for a product they have purchased.",
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
    }

    private async Task<Results<Created, BadRequest, ForbidHttpResult, UnauthorizedHttpResult>> LeaveProductFeedback(
        [FromRoute] string productId,
        [FromBody] LeaveProductFeedbackRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<LeaveProductFeedbackCommand>((productId, request));

        var result = await sender.Send(command);

        return TypedResults.Created($"/products/{productId}/feedback/{result}");
    }
}
