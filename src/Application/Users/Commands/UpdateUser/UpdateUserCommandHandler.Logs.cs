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
    private partial void LogInitiatingUserUpdate(string? id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The user could not be updated because either the user does not exist or is inactive."
    )]
    private partial void LogUserToBeUpdatedNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The user email is being updated. Former email: {FormerEmail}, New Email: {NewEmail}."
    )]
    private partial void LogEmailBeingUpdated(string formerEmail, string newEmail);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Information,
        Message = "The user could not be updated because there is already a user with the same email. Email in use: {Email}"
    )]
    private partial void LogEmailConflict(string email);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The user new email is available. Email: {Email}."
    )]
    private partial void LogEmailAvailable(string email);

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message = "The user data was updated."
    )]
    private partial void LogUserUpdated();

    [LoggerMessage(
        EventId = 7,
        Level = LogLevel.Debug,
        Message = "The user has been updated and the changes have been saved. Operation complete successfully."
    )]
    private partial void LogUserUpdatedAndSavedSuccessfully();
}
