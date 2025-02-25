using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProduct;

internal sealed partial class UpdateProductCommandHandler
{
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating product update. Product id: {Id}.")]
    private partial void LogInitiatingProductUpdate(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Product to be updated either does not exist or is inactive."
    )]
    private partial void LogProductDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Product updated and saved successfully."
    )]
    private partial void LogProductUpdatedSuccessfully();
}
