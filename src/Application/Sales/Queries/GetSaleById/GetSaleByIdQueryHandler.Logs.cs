using Microsoft.Extensions.Logging;

namespace Application.Sales.Queries.GetSaleById;

internal sealed partial class GetSaleByIdQueryHandler
{
    private readonly ILogger<GetSaleByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating retrieval of sale with identifier '{SaleId}'."
    )]
    private partial void LogInitiatingSaleRetrieval(string saleId);

    [LoggerMessage(
       EventId = 2,
       Level = LogLevel.Debug,
       Message = "The sale could not be retrieved because it was not found."
   )]
    private partial void LogSaleNotFound();

    [LoggerMessage(
       EventId = 3,
       Level = LogLevel.Debug,
       Message =
        "The sale with identifier '{SaleId}' was retrieved. " +
        "The operation was completed successfully."
   )]
    private partial void LogSaleRetrievedSuccessfully(string saleId);
}
