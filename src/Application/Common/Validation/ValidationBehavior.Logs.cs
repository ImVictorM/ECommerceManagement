using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Common.Validation;

internal sealed partial class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting validation for request of type {RequestType}."
    )]
    private partial void LogValidatingRequest(string requestType);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "No validator found for request of type {RequestType}. " +
        "Proceeding with the request."
    )]
    private partial void LogNoValidator(string requestType);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The request is valid. Proceeding to next delegate."
    )]
    private partial void LogRequestIsValid();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The request validation failed. Throwing validation exception."
    )]
    private partial void LogRequestIsInvalid();
}
