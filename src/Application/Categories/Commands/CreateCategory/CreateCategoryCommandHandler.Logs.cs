using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.CreateCategory;

internal sealed partial class CreateCategoryCommandHandler
{
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating creation of category with name {CategoryName}."
    )]
    private partial void LogInitiatingCategoryCreation(string categoryName);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The category was created and saved. " +
        "The operation completed successfully."
    )]
    private partial void LogCategoryCreatedAndSaved();
}
