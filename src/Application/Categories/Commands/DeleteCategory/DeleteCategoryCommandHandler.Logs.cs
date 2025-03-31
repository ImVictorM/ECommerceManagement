using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.DeleteCategory;

internal sealed partial class DeleteCategoryCommandHandler
{
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting removal of category with identifier '{CategoryId}'."
    )]
    private partial void LogInitiatingCategoryDeletion(string categoryId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The category could not be deleted because it does not exist."
    )]
    private partial void LogCategoryNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The category was deleted. " +
        "The operation was completed successfully."
    )]
    private partial void LogCategoryDeletedSuccessfully();
}
