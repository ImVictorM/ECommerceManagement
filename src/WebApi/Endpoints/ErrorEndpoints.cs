using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using SharedKernel.Errors;
using WebApi.Common.Extensions;

namespace WebApi.Endpoints;

/// <summary>
/// Endpoints to handle errors.
/// </summary>
public sealed class ErrorEndpoints : ICarterModule
{
    /// <summary>
    /// Base endpoint for handling errors.
    /// </summary>
    public static readonly string BaseEndpoint = "/error";

    /// <summary>
    /// Add the routes related to errors.
    /// </summary>
    /// <param name="app">The application instance.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var errorGroup = app.MapGroup(BaseEndpoint);

        errorGroup.Map("/", HandleGlobalErrors);
    }

    /// <summary>
    /// Handle all thrown exceptions.
    /// </summary>
    /// <param name="httpContext">Context of the request.</param>
    /// <returns>A <see cref="IResult"/> containing a <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> response</returns>
    private IResult HandleGlobalErrors(HttpContext httpContext)
    {
        var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var genericProblem = Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "An unexpected error ocurred.",
            detail: exception?.Message
        );

        // Handle custom errors
        return exception switch
        {
            BaseException baseException => Results.Problem(
                statusCode: (int)baseException.ErrorCode.ToHttpStatusCode(),
                title: baseException.Title,
                detail: baseException.Message,
                extensions: baseException.Context
            ),
            ValidationException validationException => HandleValidationException(validationException),
            _ => genericProblem,
        };
    }

    /// <summary>
    /// Generates an validation error response.
    /// </summary>
    /// <param name="validationException">The validation exception.</param>
    /// <returns>An error response in the <see cref="Results.ValidationProblem"/> format.</returns>
    private static IResult HandleValidationException(ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return Results.ValidationProblem(errors);
    }
}
