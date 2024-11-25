using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.CreateProduct;

public partial class CreateProductCommandHandler
{
    private readonly ILogger<CreateProductCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating product creation. Product name: {Name}"
    )]
    private partial void LogCreatingProduct(string name);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Product created successfully. Initiating persistence process."
    )]
    private partial void LogProductCreatedInitiatingPersistence();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Product persisted successfully. Generated id: {productId}. Returning result."
    )]
    private partial void LogProductPersistedSuccessfully(string? productId);

}
