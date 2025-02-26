using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProductInventory;

internal sealed partial class UpdateProductInventoryCommandHandler
{
    private readonly ILogger<UpdateProductInventoryCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating inventory update. Product id: {Id}."
    )]
    private partial void LogInitiatingInventoryUpdate(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The product inventory could not be updated. The product either does not exist or is inactive."
    )]
    private partial void LogProductDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Incrementing the inventory quantity. Current inventory quantity available: {QuantityBefore}."
    )]
    private partial void LogIncrementingQuantityInInventory(int quantityBefore);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The product inventory was updated successfully. Current inventory quantity: {QuantityAfter}."
    )]
    private partial void LogInventoryUpdatedSuccessfully(int quantityAfter);
}
