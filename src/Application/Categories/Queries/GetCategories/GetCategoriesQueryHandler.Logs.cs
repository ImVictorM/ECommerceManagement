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
    private partial void LogStartQueryingAllCategories();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "All categories fetched successfully."
    )]
    private partial void LogAllCategoriesFetched();
}
