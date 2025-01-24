using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.UpdateCategory;

public partial class UpdateCategoryCommandHandler
{
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting category update for category with id {CategoryId}"
    )]
    private partial void LogInitiatingCategoryUpdate(string categoryId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The category was not found"
    )]
    private partial void LogCategoryNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Category updated successfully"
    )]
    private partial void LogCategoryUpdated();
}
