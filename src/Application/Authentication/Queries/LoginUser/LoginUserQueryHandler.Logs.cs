using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.LoginUser;

internal sealed partial class LoginUserQueryHandler
{
    private readonly ILogger<LoginUserQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user authentication. User email: {Email}."
    )]
    private partial void LogInitiatingUserAuthentication(string email);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "Authentication failed: " +
        "Incorrect credentials supplied or user may be inactive."
    )]
    private partial void LogUserAuthenticationFailed();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Generating user authentication token."
    )]
    private partial void LogGeneratingUserAuthenticationToken();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message =
        "User with email {Email} was successfully authenticated. " +
        "The operation was completed successfully."
    )]
    private partial void LogUserAuthenticatedSuccessfully(string email);
}
