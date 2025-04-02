using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.UpdateCategory;

internal sealed partial class UpdateCategoryCommandHandler
{
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Starting category update for category with identifier '{CategoryId}'."
    )]
    private partial void LogInitiatingCategoryUpdate(string categoryId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The category could not be updated because it was not found."
    )]
    private partial void LogCategoryNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The category has been updated. " +
        "The operation was completed successfully."
    )]
    private partial void LogCategoryUpdatedSuccessfully();
}
