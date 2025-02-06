using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.LoginUser;

public partial class LoginUserQueryHandler
{
    private readonly ILogger<LoginUserQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Handling login for user with email {Email}."
    )]
    private partial void LogHandlingLoginQuery(string email);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Authentication failed: Incorrect credentials supplied or user may be inactive."
    )]
    private partial void LogAuthenticationFailed();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "User with email {Email} was successfully authenticated."
    )]
    private partial void LogSuccessfullyAuthenticatedUser(string email);
}
