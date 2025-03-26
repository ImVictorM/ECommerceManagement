using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetUsers;

internal sealed partial class GetUsersQueryHandler
{
    private readonly ILogger<GetUsersQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating users retrieval with filters - Active: {Active}."
    )]
    private partial void LogInitiatingUsersRetrieval(bool? active);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The users has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogUsersRetrievedSuccessfully();
}
