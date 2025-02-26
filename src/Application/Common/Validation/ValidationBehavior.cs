using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Validation;

internal sealed partial class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

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
