using Microsoft.Extensions.Logging;

namespace Application.Categories.Queries.GetCategoryById;

internal sealed partial class GetCategoryByIdQueryHandler
{
    private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting category retrieval. Category identifier: {CategoryId}."
    )]
    private partial void LogInitiatingCategoryRetrieval(string categoryId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The category could not be retrieved because it does not exist."
    )]
    private partial void LogCategoryNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The category was found and retrieved. " +
        "The operation completed successfully."
    )]
    private partial void LogCategoryRetrievedSuccessfully();
}
