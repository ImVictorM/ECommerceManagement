using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.UpdateUser;

public partial class UpdateUserCommandHandler
{
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user update. User id: {Id}."
    )]
    private partial void LogInitiatingUserUpdate(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The current user is not allowed to update the other user."
    )]
    private partial void LogUserNotAllowed();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Information,
        Message = "The user could not be updated because there is already a user with the same email."
    )]
    private partial void LogEmailConflict();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Updating and save user changes."
    )]
    private partial void LogUpdatingUser();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The user has been updated and the changes have been saved."
    )]
    private partial void LogUpdateComplete();
}
