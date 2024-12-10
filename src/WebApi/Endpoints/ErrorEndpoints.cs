using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public const string BaseEndpoint = "/error";


    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var errorGroup = app.MapGroup(BaseEndpoint);

        errorGroup.Map("/", HandleGlobalErrors);
    }

    private Results<ProblemHttpResult, ValidationProblem> HandleGlobalErrors(HttpContext httpContext)
    {
        var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var genericProblem = TypedResults.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "An unexpected error occurred.",
            detail: exception?.Message
        );

        return exception switch
        {
            BaseException baseException => TypedResults.Problem(
                statusCode: (int)baseException.ErrorCode.ToHttpStatusCode(),
                title: baseException.Title,
                detail: baseException.Message,
                extensions: baseException.Context
            ),
            ValidationException validationException => HandleValidationException(validationException),
            _ => genericProblem,
        };
    }

    private static ValidationProblem HandleValidationException(ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return TypedResults.ValidationProblem(errors);
    }
}
