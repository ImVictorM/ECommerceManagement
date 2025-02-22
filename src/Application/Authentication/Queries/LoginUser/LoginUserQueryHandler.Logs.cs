using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.LoginUser;

public partial class LoginUserQueryHandler
{
    private readonly ILogger<LoginUserQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user authentication. User email: {Email}."
    )]
    private partial void LogInitiatingUserLogin(string email);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Authentication failed: Incorrect credentials supplied or user may be inactive."
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
        Message = "User with email {Email} was successfully authenticated."
    )]
    private partial void LogUserAuthenticatedSuccessfully(string email);
}
