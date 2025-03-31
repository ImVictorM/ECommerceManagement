using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetUserById;

internal sealed partial class GetUserByIdQueryHandler
{
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user retrieval. User identifier: '{Id}'."
    )]
    private partial void LogInitiatingUserRetrieval(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The user could not be retrieved because they were not found."
    )]
    private partial void LogUserNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The user has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogUserRetrievedSuccessfully();
}
