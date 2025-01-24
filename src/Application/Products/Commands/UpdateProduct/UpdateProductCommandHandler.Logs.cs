using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProduct;

public partial class UpdateProductCommandHandler
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
        Message = "Product to be deactivated either does not exist or is already inactive."
    )]
    private partial void LogProductDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Product updated and saved successfully."
    )]
    private partial void LogProductUpdatedSuccessfully();
}
