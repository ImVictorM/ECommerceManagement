using Carter;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.Endpoints;

/// <summary>
/// Endpoints to handle errors.
/// </summary>
public sealed class ErrorEndpoints : CarterModule
{
    /// <summary>
    /// Base endpoint for handling errors.
    /// </summary>
    public const string BaseEndpoint = "/error";

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorEndpoints"/> class.
    /// </summary>
    public ErrorEndpoints() : base(BaseEndpoint) { }

    /// <inheritdoc/>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.Map("/", HandleGlobalErrors);
    }

    /// <summary>
    /// Handle all thrown exceptions.
    /// </summary>
    /// <param name="httpContext">Context of the request.</param>
    /// <returns>A <see cref="IResult"/> containing a <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> response</returns>
    private IResult HandleGlobalErrors(HttpContext httpContext)
    {
        Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is null)
        {
            // Handle unexpected errors
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error ocurred."
            );
        }

        // Handle custom errors
        return exception switch
        {
            _ => Results.Problem(),
        };
    }
}
