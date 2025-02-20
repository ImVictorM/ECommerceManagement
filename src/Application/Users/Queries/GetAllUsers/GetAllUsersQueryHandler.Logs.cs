using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetAllUsers;

public partial class GetAllUsersQueryHandler
{
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating users retrieval."
    )]
    private partial void LogInitiatingUsersRetrieval();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Active filter: {ActiveFilter}."
    )]
    private partial void LogActiveFilter(string activeFilter);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The users were retrieved successfully."
    )]
    private partial void LogUsersRetrieved();
}
