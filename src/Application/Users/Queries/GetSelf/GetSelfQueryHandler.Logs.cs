using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetSelf;

internal sealed partial class GetSelfQueryHandler
{
    private readonly ILogger<GetSelfQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user self retrieval."
    )]
    private partial void LogInitiatingSelfRetrieval();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Current user identifier: '{UserId}'."
    )]
    private partial void LogCurrentUserId(string userId);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Error,
        Message =
        "The current user could not be found internally. " +
        "The operation failed."
    )]
    private partial void LogCurrentUserNotFoundInternally();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message =
        "The user data has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogCurrentUserInfoRetrieved();
}
