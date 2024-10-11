using Microsoft.Extensions.Logging;

namespace Application.Authentication.Commands.Register;

public partial class RegisterCommandHandler
{
    /// <summary>
    /// Logger to handle logging.
    /// </summary>
    private readonly ILogger<RegisterCommandHandler> _logger;

    /// <summary>
    /// Log the start of the process to create a new user.
    /// </summary>
    /// <param name="email">The user email.</param>
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Initiating user creation with email {Email}"
    )]
    public partial void LogHandlingRegisterCommand(string email);

    /// <summary>
    /// Log a warning when trying to register a user that already exists.
    /// </summary>
    /// <param name="email">The email that generated the conflict.</param>
    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "There was an error when trying to create a new user with email {Email}. The email already exists."
    )]
    public partial void LogUserAlreadyExists(string email);

    /// <summary>
    /// Log an information saying that the user was instantiate successfully and associated with the customer role.
    /// </summary>
    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Information,
        Message = "User was created and associated with the customer role. Initiating persistence process"
    )]
    public partial void LogUserCreatedWithCustomerRole();

    /// <summary>
    /// Log an information saying that the user was successfully persisted in the database.
    /// </summary>
    /// <param name="email">The user email.</param>
    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Information,
        Message = "User with email {Email} was saved successfully. Initiating token generation"
    )]
    public partial void LogUserSavedSuccessfully(string email);

    /// <summary>
    /// Log an information saying that a token was generated successfully for the recent created user.
    /// </summary>
    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Information,
        Message = "Token generated successfully"
    )]
    public partial void LogTokenGeneratedSuccessfully();
}
