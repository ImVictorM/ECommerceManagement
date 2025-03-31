using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.LoginCarrier;

internal sealed partial class LoginCarrierQueryHandler
{
    private readonly ILogger<LoginCarrierQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating carrier authentication."
    )]
    private partial void LogInitiatingCarrierAuthentication();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Authentication failed: Email or password incorrect."
    )]
    private partial void LogCarrierAuthenticationFailed();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "Generating carrier authentication token." +
        " Carrier identifier: {CarrierId}."
    )]
    private partial void LogGeneratingCarrierAuthenticationToken(string carrierId);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message =
        "The carrier was authenticated. " +
        "The operation was completed successfully."
    )]
    private partial void LogCarrierAuthenticatedSuccessfully();
}
