using Microsoft.Extensions.Logging;

namespace Application.Sales.Commands.DeleteSale;

internal sealed partial class DeleteSaleCommandHandler
{
    private readonly ILogger<DeleteSaleCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating deletion for sale with id '{SaleId}'."
    )]
    private partial void LogInitiatingSaleDeletion(string saleId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The sale could not be deleted because it was not found."
    )]
    private partial void LogSaleNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The sale with id '{SaleId}' was deleted. " +
        "The operation was completed successfully."
    )]
    private partial void LogSaleDeletedSuccessfully(string saleId);
}
