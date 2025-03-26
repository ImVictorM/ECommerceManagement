using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.DeactivateUser;

internal sealed partial class DeactivateUserCommandHandler
{
    private readonly ILogger<DeactivateUserCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user deactivation. User identifier: {Id}."
    )]
    private partial void LogInitiatingUserDeactivation(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The user to be deactivated either does not exist or is already inactive."
    )]
    private partial void LogUserNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The user has been deactivated and is no longer active. " +
        "The operation was completed successfully."
    )]
    private partial void LogDeactivationComplete();
}
