using Microsoft.Extensions.Logging;

namespace Application.Sales.Queries.GetSales;

internal sealed partial class GetSalesQueryHandler
{
    private readonly ILogger<GetSalesQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating retrieval of sales with filters: " +
        "ExpiringAfter={ExpiringAfter}, " +
        "ExpiringBefore={ExpiringBefore}, " +
        "ValidForDate={ValidForDate}"
    )]
    private partial void LogInitiatingSalesRetrieval(
        DateTimeOffset? expiringAfter,
        DateTimeOffset? expiringBefore,
        DateTimeOffset? validForDate
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "{Count} sales were retrieved. The operation was completed successfully."
    )]
    private partial void LogSalesRetrievedSuccessfully(int count);
}
