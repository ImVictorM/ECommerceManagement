using Application.ProductFeedback.Commands.DeactivateCustomerProductFeedback;
using Application.ProductFeedback.Queries.GetCustomerProductFeedback;

using Contracts.ProductFeedback;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.ProductFeedback;

/// <summary>
/// Provides endpoints for the customer product feedback features.
/// </summary>
public class CustomerProductFeedbackEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var customerProductFeedbackGroup = app
            .MapGroup("/users/customers/{userId:long}/feedback")
            .WithTags("ProductFeedback")
            .WithOpenApi();

        customerProductFeedbackGroup
            .MapGet("/", GetCustomerProductFeedback)
            .WithName(nameof(GetCustomerProductFeedback))
            .WithOpenApi(op => new(op)
            {
                Summary = "Get Customer Product Feedback",
                Description =
                "Retrieves a list of active product feedback items left " +
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

        customerProductFeedbackGroup
            .MapDelete("/{feedbackId:long}", DeactivateCustomerProductFeedback)
            .WithName(nameof(DeactivateCustomerProductFeedback))
            .WithOpenApi(op => new(op)
            {
                Summary = "Deactivate Customer Product Feedback",
                Description =
                "Deactivates a specific product feedback entry from a customer. " +
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
                        Name = "feedbackId",
                        In = ParameterLocation.Path,
                        Description = "The feedback item identifier.",
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
        Ok<IEnumerable<ProductFeedbackResponse>>,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> GetCustomerProductFeedback(
        [FromRoute] string userId,
        IMapper mapper,
        ISender sender
    )
    {
        var query = new GetCustomerProductFeedbackQuery(userId);

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<ProductFeedbackResponse>));
    }

    internal async Task<Results<
        NoContent,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> DeactivateCustomerProductFeedback(
        [FromRoute] string userId,
        [FromRoute] string feedbackId,
        ISender sender
    )
    {
        var query = new DeactivateCustomerProductFeedbackCommand(
            userId,
            feedbackId
        );

        await sender.Send(query);

        return TypedResults.NoContent();
    }
}
