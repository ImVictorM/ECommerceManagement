using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.CreateCategory;

public partial class CreateCategoryCommandHandler
{
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating creation of category with name {CategoryName}."
    )]
    private partial void LogCreatingCategory(string categoryName);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Category created and saved successfully."
    )]
    private partial void LogCategoryCreatedAndSaved();
}
