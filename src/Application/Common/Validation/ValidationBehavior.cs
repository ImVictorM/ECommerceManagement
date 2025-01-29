using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Validation;

/// <summary>
/// A pipeline behavior that performs validation before the request is handled.
/// This behavior ensures that the request input passes validation before being passed
/// to the next handler in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public partial class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validator">
    /// An optional validator for the request. If no validator is provided, the request will not
    /// be validated.
    /// </param>
    /// <param name="logger">The logger.</param>
    public ValidationBehavior(
        ILogger<ValidationBehavior<TRequest, TResponse>> logger,
        IValidator<TRequest>? validator = null
    )
    {
        _logger = logger;
        _validator = validator;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        LogValidatingRequest(typeof(TRequest).Name);

        if (_validator is null)
        {
            LogNoValidator(typeof(TRequest).Name);

            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            LogRequestIsValid();

            return await next();
        }

        LogRequestIsInvalid();
        throw new ValidationException(validationResult.Errors);
    }
}
