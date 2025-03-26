using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetOrders;

internal sealed partial class GetOrdersQueryHandler
{
    private readonly ILogger<GetOrdersQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating orders retrieval with filters: " +
        "Status={Status}"
    )]
    private partial void LogInitiatingOrdersRetrieval(
        string? status
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "{QuantityFound} orders has been retrieved. " +
        "The operation completed successfully."
    )]
    private partial void LogOrdersRetrievedSuccessfully(int quantityFound);
}
