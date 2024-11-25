using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProductDiscounts;

public partial class UpdateProductDiscountsCommandHandler
{
    private readonly ILogger<UpdateProductDiscountsCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating product discounts update. Product id: {Id}."
    )]
    private partial void LogInitiatingProductDiscountsUpdate(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "It was not possible to update the discounts on the product. The product either does not exist or is inactive."
    )]
    private partial void LogProductDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Clearing the current discounts on the product. Current discounts count: {Count}."
    )]
    private partial void LogClearingCurrentDiscounts(int count);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Discounts cleared. Adding the new discounts to the product. New discounts count: {Count}."
   )]
    private partial void LogAddingNewDiscounts(int count);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The discounts on the product was updated and saved successfully."
    )]
    private partial void LogDiscountsUpdatedSuccessfully();
}
