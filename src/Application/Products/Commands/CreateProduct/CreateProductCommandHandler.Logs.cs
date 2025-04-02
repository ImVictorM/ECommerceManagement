using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.CreateProduct;

internal sealed partial class CreateProductCommandHandler
{
    private readonly ILogger<CreateProductCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating product creation. Product name: {Name}."
    )]
    private partial void LogInitiatingProductCreation(string name);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The product object has been created. " +
        "Initiating persistence process."
    )]
    private partial void LogProductCreatedInitiatingPersistence();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The product has been created and saved with the identifier '{ProductId}'. " +
        "The operation was completed successfully."
    )]
    private partial void LogProductPersistedSuccessfully(string? productId);

}
