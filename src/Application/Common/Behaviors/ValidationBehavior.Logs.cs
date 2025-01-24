using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public partial class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
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
        Message = "No validator found for request of type {RequestType}. Proceeding to next delegate."
    )]
    private partial void LogNoValidator(string requestType);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Validation succeeded. Proceeding to next delegate."
    )]
    private partial void LogRequestIsValid();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Validation failed. Throwing validation exception."
    )]
    private partial void LogRequestIsInvalid();
}
