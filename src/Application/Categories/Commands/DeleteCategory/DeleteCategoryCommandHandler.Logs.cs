using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.DeleteCategory;

public partial class DeleteCategoryCommandHandler
{
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting removal of category with id {CategoryId}"
    )]
    private partial void LogInitiatingDeleteCategory(string categoryId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The category was not found"
    )]
    private partial void LogCategoryNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The category was found an removed successfully"
    )]
    private partial void LogCategoryDeleted();
}
