using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.DeleteCategory;

internal sealed partial class DeleteCategoryCommandHandler
{
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting removal of category with id {CategoryId}."
    )]
    private partial void LogInitiatingDeleteCategory(string categoryId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The could not be deleted because it does not exist."
    )]
    private partial void LogCategoryNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The category was found an removed successfully."
    )]
    private partial void LogCategoryDeleted();
}
