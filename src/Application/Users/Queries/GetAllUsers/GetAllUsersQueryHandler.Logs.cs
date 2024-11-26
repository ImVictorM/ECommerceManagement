using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetAllUsers;

public partial class GetAllUsersQueryHandler
{
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating user retrieval."
    )]
    private partial void LogInitiatingUsersRetrieval();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Users retrieved.\nFilters: active={Active}."
    )]
    private partial void LogUsersRetrieved(bool? active);
}
