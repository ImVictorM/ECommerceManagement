using Application.Common.Security.Authorization.Requests;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Security.Authorization;

internal sealed partial class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequestWithAuthorization<TResponse>
{
    private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting authorization for request of type {RequestType}"
    )]
    private partial void LogAuthorizingRequest(string requestType);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "No authorization attributes were found for the request. User is authorized"
    )]
    private partial void LogNoAuthorizationAttributesFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "{Quantity} required roles were found"
    )]
    private partial void LogRequiredRoles(int quantity);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "{Quantity} required policies were found"
    )]
    private partial void LogRequiredPolicies(int quantity);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "Checking if the current user is authorized..."
    )]
    private partial void LogCheckingUserAuthorization();

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message = "User is authorized. Proceeding to next delegate"
    )]
    private partial void LogUserIsAuthorized();

    [LoggerMessage(
        EventId = 7,
        Level = LogLevel.Debug,
        Message = "User is not authorized. Throwing authorization exception"
    )]
    private partial void LogUserIsNotAuthorized();
}
