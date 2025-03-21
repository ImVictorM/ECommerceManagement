using Microsoft.Extensions.Logging;

namespace Application.Sales.Commands.UpdateSale;

internal sealed partial class UpdateSaleCommandHandler
{
    private readonly ILogger<UpdateSaleCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating update for sale with id '{SaleId}'."
    )]
    private partial void LogInitiatingSaleUpdate(string saleId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The sale could not be updated because it was not found."
    )]
    private partial void LogSaleNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The sale entity was updated. Checking eligibility of sale products..."
    )]
    private partial void LogSaleUpdated();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The sale products are eligible for the new sale."
    )]
    private partial void LogSaleProductsIsEligible();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message =
        "The sale was update and saved. " +
        "The operation was completed successfully."
    )]
    private partial void LogSaleUpdatedAndSavedSuccessfully();
}
