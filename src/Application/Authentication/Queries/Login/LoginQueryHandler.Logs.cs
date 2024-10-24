using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.Login;

public partial class LoginQueryHandler
{
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
    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "Authentication failed: Incorrect credentials supplied or user may be inactive"
    )]
    public partial void LogAuthenticationFailed();


    /// <summary>
    /// Log an information when the user email and password was correct.
    /// </summary>
    /// <param name="email">The user email.</param>
    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Information,
        Message = "User with email {Email} was succesfully authenticated. Initiating token generation."
    )]
    public partial void LogSuccessfullyAuthenticatedUser(string email);

    /// <summary>
    /// Log an information saying that the token was successfully generated for the user.
    /// </summary>
    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Information,
        Message = "JWT token generated successfully"
    )]
    public partial void LogTokenGenerated();
}
