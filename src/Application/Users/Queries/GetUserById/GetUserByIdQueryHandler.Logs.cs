using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetUserById;

public partial class GetUserByIdQueryHandler
{
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user fetch. User id: {Id}."
    )]
    private partial void LogInitiatingUserFetch(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The user was not found."
    )]
    private partial void LogUserNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The user was found. Returning them."
    )]
    private partial void LogUserFound();
}
