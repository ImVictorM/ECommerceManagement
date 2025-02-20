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
        Message = "The user to be deactivated either does not exist or is already inactive."
    )]
    private partial void LogUserDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Deactivation complete. The User is no longer active."
    )]
    private partial void LogDeactivationComplete();
}
