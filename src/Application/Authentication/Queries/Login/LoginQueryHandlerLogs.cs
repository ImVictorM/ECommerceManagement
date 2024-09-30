using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.Login;

public partial class LoginQueryHandler
{
    /// <summary>
    /// Logger to handle logging.
    /// </summary>
    private readonly ILogger<LoginQueryHandler> _logger;

    /// <summary>
    /// Log the initialization of the login process.
    /// </summary>
    /// <param name="email">The email of the user trying to login.</param>
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Handling login for user with email {Email}"
    )]
    public partial void LogHandlingLoginQuery(string email);

    /// <summary>
    /// Log a warning when the user trying to login is not found using their email.
    /// </summary>
    /// <param name="email">The email of the user trying to login.</param>
    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "Login failed: User with email {Email} not found"
    )]
    public partial void LogUserNotFound(string email);


    /// <summary>
    /// Log a warning when the user trying to login passed wrong password credentials.
    /// </summary>
    /// <param name="email">The email of the user trying to login.</param>
    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Warning,
        Message = "Login failed: Incorrect password for email {Email}"
    )]
    public partial void LogInvalidPassword(string email);

    /// <summary>
    /// Log an information when the user email and password was correct.
    /// </summary>
    /// <param name="email"></param>
    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Information,
        Message = "User with email {Email} was succesfully authenticated. Initiating token generation."
    )]
    public partial void LogSuccessfullyAuthenticatedUser(string email);

    /// <summary>
    /// Log an information saying that the token was successfully generated for the user.
    /// </summary>
    /// <param name="userId">The user id the token was generated.</param>
    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Information,
        Message = "JWT token generated for user with identifier {UserId}"
    )]
    public partial void LogTokenGenerated(long userId);
}
