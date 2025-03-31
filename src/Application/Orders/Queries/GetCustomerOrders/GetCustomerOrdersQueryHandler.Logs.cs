using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetCustomerOrders;

internal sealed partial class GetCustomerOrdersQueryHandler
{
    private readonly ILogger<GetCustomerOrdersQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating customer orders retrieval for customer with identifier " +
        "'{OwnerId}' containing filters: " +
        "Status={Status}"
    )]
    private partial void LogInitiatingCustomerOrdersRetrieval(
        string ownerId,
        string? status
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "{QuantityFound} orders has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogOrdersRetrievedSuccessfully(int quantityFound);
}
