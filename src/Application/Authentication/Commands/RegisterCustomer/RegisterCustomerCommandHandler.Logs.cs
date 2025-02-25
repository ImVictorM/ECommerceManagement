using Microsoft.Extensions.Logging;

namespace Application.Authentication.Commands.RegisterCustomer;

internal sealed partial class RegisterCustomerCommandHandler
{
    private readonly ILogger<RegisterCustomerCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating customer registration. " +
        "Customer email: {Email}."
    )]
    private partial void LogInitiatingRegisterCustomer(string email);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "There was an error when trying to register the customer." +
        " The customer email is already in use."
    )]
    private partial void LogEmailAlreadyInUse();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The customer was created successfully."
    )]
    private partial void LogCustomerCreated();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The customer saved successfully."
    )]
    private partial void LogCustomerSavedSuccessfully();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The authentication token was generated successfully."
    )]
    private partial void LogAuthenticationTokenGenerated();

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message = "The customer was registered and the authentication token was generated. " +
        "The process complete successfully."
    )]
    private partial void LogRegistrationCompleteSuccessfully();
}
