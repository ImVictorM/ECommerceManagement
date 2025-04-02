using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.AddStock;

internal sealed partial class AddStockCommandHandler
{
    private readonly ILogger<AddStockCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Starting to increase the stock quantity of the product" +
        " with identifier '{Id}'."
    )]
    private partial void LogInitiatingAddingStock(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The stock could not be added. " +
        "The product either does not exist or is inactive."
    )]
    private partial void LogProductDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "Increasing stock quantity. " +
        "Current quantity available: {QuantityBefore}."
    )]
    private partial void LogAddingStock(int quantityBefore);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message =
        "The stock quantity has been increased to {QuantityAfter}. " +
        "The operation was completed successfully."
    )]
    private partial void LogStockAddedSuccessfully(int quantityAfter);
}
