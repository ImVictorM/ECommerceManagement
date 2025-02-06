using Microsoft.Extensions.Logging;

namespace Application.Authentication.Commands.RegisterCustomer;

public partial class RegisterCustomerCommandHandler
{
    private readonly ILogger<RegisterCustomerCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user creation with email {Email}."
    )]
    private partial void LogHandlingRegisterCommand(string email);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "There was an error when trying to create a new user. The user already exists."
    )]
    private partial void LogUserAlreadyExists();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "User was created and associated with the customer role. Initiating persistence process."
    )]
    private partial void LogUserCreatedWithCustomerRole();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "User with email {Email} was saved successfully. Initiating token generation."
    )]
    private partial void LogUserSavedSuccessfully(string email);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "Token generated successfully."
    )]
    private partial void LogTokenGeneratedSuccessfully();
}
