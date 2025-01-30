using WebApi.Common.Extensions;

using SharedKernel.Errors;

using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Common.Middleware;

/// <summary>
/// Middleware to handle exceptions globally.
/// </summary>
public sealed class GlobalExceptionHandler : ICarterModule
{
    /// <summary>
    /// The middleware base endpoint.
    /// </summary>
    public const string BaseEndpoint = "/error";

    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.Map(BaseEndpoint, Handle);
    }

    private Results<ProblemHttpResult, ValidationProblem> Handle(HttpContext httpContext)
    {
        var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return exception switch
        {
            BaseException baseException => CreateBaseExceptionProblem(baseException),
            ValidationException validationException => CreateValidationProblem(validationException),
            _ => CreateGenericProblem(exception),
        };
    }

    private static ProblemHttpResult CreateBaseExceptionProblem(BaseException baseException)
    {
        return TypedResults.Problem(
            statusCode: (int)baseException.ErrorCode.ToHttpStatusCode(),
            title: baseException.Title,
            detail: baseException.Message,
            extensions: baseException.Context
        );
    }

    private static ValidationProblem CreateValidationProblem(ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return TypedResults.ValidationProblem(errors);
    }

    private static ProblemHttpResult CreateGenericProblem(Exception? exception)
    {
        return TypedResults.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "An unexpected error occurred.",
            detail: exception?.Message
        );
    }
}
