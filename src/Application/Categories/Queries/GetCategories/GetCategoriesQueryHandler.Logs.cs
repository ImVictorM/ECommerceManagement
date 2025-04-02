using Microsoft.Extensions.Logging;

namespace Application.Categories.Queries.GetCategories;

internal sealed partial class GetCategoriesQueryHandler
{
    private readonly ILogger<GetCategoriesQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting query for all the categories."
    )]
    private partial void LogInitiatingCategoriesRetrieval();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "{Quantity} categories has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogCategoriesRetrievedSuccessfully(int quantity);
}
