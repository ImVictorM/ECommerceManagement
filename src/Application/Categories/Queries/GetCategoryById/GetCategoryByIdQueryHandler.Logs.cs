using Microsoft.Extensions.Logging;

namespace Application.Categories.Queries.GetCategoryById;

internal sealed partial class GetCategoryByIdQueryHandler
{
    private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting category search by id. Category id: {CategoryId}."
    )]
    private partial void LogInitiatingGetCategory(string categoryId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The category was not found."
    )]
    private partial void LogCategoryNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The category was found and retrieved successfully."
    )]
    private partial void LogCategoryRetrieved();
}
