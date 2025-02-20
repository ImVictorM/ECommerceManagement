using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetSelf;

public sealed partial class GetSelfQueryHandler
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
        Message = "Current user id: {UserId}."
    )]
    private partial void LogCurrentUserId(string userId);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Error,
        Message = "The current user could not be found internally."
    )]
    private partial void LogCurrentUserNotFoundInternally();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "User data retrieved successfully."
    )]
    private partial void LogCurrentUserInfoRetrieved();
}
