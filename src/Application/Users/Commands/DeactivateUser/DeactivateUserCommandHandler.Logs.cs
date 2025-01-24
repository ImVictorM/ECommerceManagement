using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.DeactivateUser;

public partial class DeactivateUserCommandHandler
{
    private readonly ILogger<DeactivateUserCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user deactivation. User to be deactivated id: {Id}."
    )]
    private partial void LogInitiatingUserDeactivation(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The user to be deactivated does not exist."
    )]
    private partial void LogUserDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Information,
        Message = "A user without the right privileges tried to deactivate another user. Current user id: {Id}"
    )]
    private partial void LogUserNotAllowed(string id);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Deactivation complete. User is no longer available."
    )]
    private partial void LogDeactivationComplete();
}
