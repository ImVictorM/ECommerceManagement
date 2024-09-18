using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;

/// <summary>
/// A pipeline behavior that performs validation before the request is handled.
/// This behavior ensures that the request input passes validation before being passed
/// to the next handler in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// The validator associated with the request.
    /// </summary>
    private readonly IValidator<TRequest>? _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validator">
    /// An optional validator for the request. If no validator is provided, the request will not
    /// be validated.
    /// </param>
    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    /// <summary>
    /// Handles the validation of the request before it reaches the next handler in the pipeline.
    /// </summary>
    /// <param name="request">The request to be handled.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">A cancellation token for the request.</param>
    /// <returns>The response from the next handler if the request passes validation.</returns>
    /// <exception cref="ValidationException">
    /// Thrown if the request fails validation.
    /// </exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        throw new ValidationException(validationResult.Errors);
    }
}
